using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TranslationManagement.Api.Managers;
using TranslationManagement.Api.DAO;
using TranslationManagement.Api.Models;
using External.ThirdParty.Services;
using static TranslationManagement.Api.Utils.CommonUtils;
using Microsoft.Extensions.Logging;
using System;

namespace TranslationManagement.Api.Tests
{
    public class TranslationJobManagerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateJob_OnValidPayload_CreateJob()
        {
            //Arrange
            var translationJob = new TranslationJobModel()
            {
                CustomerName = "Test",
                OriginalContent = "Testsafasf",
                Price = 1,
                Status = JobStatus.New,
                TranslatedContent = ""
            };
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.CreateJob(translationJob)).Returns(translationJob);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);
            var job = translationJobManager.CreateJob(translationJob);

            //Assert
            Mock.Assert(() => translationJobDao.CreateJob(Arg.IsAny<TranslationJobModel>()), Occurs.Once());
            Assert.That(job, Is.EqualTo(translationJob));
        }

        [Test]
        public void CreateJob_OnJobStatusDifferentThanNew_SetJobStatusToNewAndCreate()
        {
            //Arrange
            var translationJob = new TranslationJobModel()
            {
                CustomerName = "Test",
                OriginalContent = "Testsafasf",
                Price = 1,
                Status = JobStatus.Inprogress,
                TranslatedContent = ""
            };
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.CreateJob(translationJob)).Returns(translationJob);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);
            var job = translationJobManager.CreateJob(translationJob);

            //Assert
            Mock.Assert(() => translationJobDao.CreateJob(Arg.IsAny<TranslationJobModel>()), Occurs.Once());
            Assert.That(job, Is.EqualTo(translationJob));
            Assert.That(job.Status, Is.EqualTo(JobStatus.New));
        }

        [Test]
        public void CreateJob_OnOriginalText_SetPriceAndCreate()
        {
            //Arrange
            var translationJob = new TranslationJobModel()
            {
                CustomerName = "Test",
                OriginalContent = "ContentOfLength17",
                Price = 0,
                Status = JobStatus.Inprogress,
                TranslatedContent = ""
            };
            double price = 0.17;
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.CreateJob(translationJob)).Returns(translationJob);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);
            var job = translationJobManager.CreateJob(translationJob);

            //Assert
            Mock.Assert(() => translationJobDao.CreateJob(Arg.IsAny<TranslationJobModel>()), Occurs.Once());
            Assert.That(job, Is.EqualTo(translationJob));
            Assert.That(price, Is.EqualTo(job.Price));
        }

        [Test]
        public void UpdateJobStatus_OnUpdateRequestValid_Update()
        {
            //Arrange
            var translator = new TranslatorModel()
            {
                Id = 1,
                Name = "Test",
                Status = TranslatorStatus.Certified,
                HourlyRate = 10,
                CreditCardNumber = "54545465466"
            };
            var jobId = 1;
            var translatorId = 2;
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()));
            Mock.Arrange(() => translatorsDao.GetTranslatorById(translatorId)).Returns(translator);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);
            translationJobManager.UpdateJobStatus(jobId, translatorId, JobStatus.New);

            //Assert
            Mock.Assert(() => translatorsDao.GetTranslatorById(translatorId), Occurs.Once());
            Mock.Assert(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()), Occurs.Once());
        }

        [Test]
        public void UpdateJobStatus_OnTranslatorNotFound_ThrowsArgumentNullException()
        {
            //Arrange
            TranslatorModel translator = null;
            var jobId = 1;
            var translatorId = 2;
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()));
            Mock.Arrange(() => translatorsDao.GetTranslatorById(translatorId)).Returns(translator);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);
            
            //Assert
            Assert.Throws<ArgumentNullException>(() => translationJobManager.UpdateJobStatus(jobId, translatorId, JobStatus.New));
            Mock.Assert(() => translatorsDao.GetTranslatorById(translatorId), Occurs.Once());
            Mock.Assert(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()), Occurs.Never());
        }

        [Test]
        public void UpdateJobStatus_OnTranslatorNotCertified_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            var translator = new TranslatorModel()
            {
                Id = 1,
                Name = "Test",
                Status = TranslatorStatus.Applicant,
                HourlyRate = 10,
                CreditCardNumber = "54545465466"
            };
            var jobId = 1;
            var translatorId = 2;
            var translationJobDao = Mock.Create<ITranslationJobsDao>();
            var translatorsDao = Mock.Create<ITranslatorsDao>();
            var loggerMock = Mock.Create<ILogger<TranslationJobManager>>();
            Mock.Arrange(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()));
            Mock.Arrange(() => translatorsDao.GetTranslatorById(translatorId)).Returns(translator);

            //Act
            var translationJobManager = new TranslationJobManager(translationJobDao, translatorsDao, loggerMock);

            //Assert
            Assert.Throws<UnauthorizedAccessException>(() => translationJobManager.UpdateJobStatus(jobId, translatorId, JobStatus.New));
            Mock.Assert(() => translatorsDao.GetTranslatorById(translatorId), Occurs.Once());
            Mock.Assert(() => translationJobDao.UpdateJobStatus(jobId, Arg.IsAny<JobStatus>()), Occurs.Never());
        }
    }
}