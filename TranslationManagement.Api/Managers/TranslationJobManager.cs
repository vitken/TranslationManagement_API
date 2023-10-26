using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.DAO;
using static TranslationManagement.Api.Utils.CommonUtils;
using TranslationManagement.Api.Controllers;
using System.Threading.Tasks;
using External.ThirdParty.Services;
using System.Threading;
using System.IO;
using System.Xml.Linq;

namespace TranslationManagement.Api.Managers
{
    public class TranslationJobManager : ITranslationJobManager
    {
        private readonly ILogger<TranslationJobManager> _logger;
        private readonly ITranslationJobsDao _translationJobsDao;
        private readonly ITranslatorsDao _translatorsDao;

        public TranslationJobManager(ITranslationJobsDao translationJobsDao, ITranslatorsDao translatorsDao, ILogger<TranslationJobManager> logger) 
        {
            _translationJobsDao = translationJobsDao;
            _translatorsDao = translatorsDao;
            _logger = logger;
        }

        public TranslationJobModel CreateJob(TranslationJobModel job)
        {
            TranslationJobModel createdJob = null;
            job.Status = JobStatus.New;
            SetPrice(job);

            try
            {
                createdJob = _translationJobsDao.CreateJob(job);

                try
                {
                    Notify(createdJob.Id);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Unreliable service was really unreliable this time..");
                }
                
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while creating new job!");
            }

            return createdJob;
        }

        public TranslationJobModel CreateJobWithFile(IFormFile file, string customer)
        {
            var reader = new StreamReader(file.OpenReadStream());
            string content;

            if (file.FileName.EndsWith(".txt"))
            {
                content = reader.ReadToEnd();
            }
            else if (file.FileName.EndsWith(".xml"))
            {
                var xdoc = XDocument.Parse(reader.ReadToEnd());
                content = xdoc.Root.Element("Content").Value;
                customer = xdoc.Root.Element("Customer").Value.Trim();
            }
            else
            {
                throw new NotSupportedException("Unsupported file");
            }

            var job = new TranslationJobModel() { 
                CustomerName = customer,
                Status = JobStatus.New,
                OriginalContent = content,
                TranslatedContent = ""
            };
            SetPrice(job);

            return CreateJob(job);
        }

        public List<TranslationJobModel> GetJobs()
        {
            return _translationJobsDao.GetJobs();
        }

        public void UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
        {
            var translator = _translatorsDao.GetTranslatorById(translatorId);
            if (translator == null)
            {
                _logger.LogError($"The translator with id {translatorId} doesn't exists!");
                throw new ArgumentNullException($"The translator with id {translatorId} doesn't exists!");
            }

            if(translator.Status != TranslatorStatus.Certified) {
                _logger.LogError($"The translator with id {translatorId} isn't certified!");
                throw new UnauthorizedAccessException($"The translator with id {translatorId} isn't certified");
            }

            _translationJobsDao.UpdateJobStatus(jobId, newStatus);
        }

        private void SetPrice(TranslationJobModel job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }

        private void Notify(int jobId)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var notificationSvc = new UnreliableNotificationService();
            while (true)
            {
                if (notificationSvc.SendNotification($"Job created: {jobId}").Result)
                {
                    _logger.LogInformation("New job notification sent");
                    break;
                }
                tokenSource.Token.ThrowIfCancellationRequested();
            }
        }
    }
}
