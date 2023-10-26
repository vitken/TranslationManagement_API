using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Managers;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly ITranslationJobManager _translationJobManager;

        public TranslationJobController(ITranslationJobManager translationJobManager, ILogger<TranslatorManagementController> logger)
        {
            _translationJobManager = translationJobManager;
            _logger = logger;
        }

        [HttpGet]
        public TranslationJobModel GetJobs()
        {
            //return _translationJobManager.GetJobs();
            return new TranslationJobModel()
            {
                CustomerName = "Viktor",
                Status = JobStatus.Inprogress
            };
        }

        [HttpPost]
        public TranslationJobModel CreateJob(TranslationJobModel job)
        {
            return _translationJobManager.CreateJob(job);
        }

        [HttpPost]
        public TranslationJobModel CreateJobWithFile(IFormFile file, string customer)
        {
            return _translationJobManager.CreateJobWithFile(file, customer);
        }

        [HttpPost]
        public IActionResult UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
        {
            _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
            try
            {
                _translationJobManager.UpdateJobStatus(jobId, translatorId, newStatus);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("The translator with given id doesn't exists!");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("The translator isn't certified!");
            }
            return Ok();
        }
    }
}