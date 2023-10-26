using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TranslationManagement.Api.Models;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Managers
{
    public interface ITranslationJobManager
    {
        /// <summary>
        /// Get all jobs.
        /// </summary>
        /// <returns></returns>
        List<TranslationJobModel> GetJobs();

        /// <summary>
        /// Creates job from given payload.
        /// </summary>
        /// <param name="job">Jot to translation</param>
        /// <returns>Created job</returns>
        TranslationJobModel CreateJob(TranslationJobModel job);

        /// <summary>
        /// Creates job by providing file (currently supported formats are .txt and .xml)
        /// </summary>
        /// <param name="file">File with content</param>
        /// <param name="customer"></param>
        /// <returns>Created job</returns>
        TranslationJobModel CreateJobWithFile(IFormFile file, string customer);

        /// <summary>
        /// Updates job status
        /// </summary>
        /// <param name="jobId">The job to update</param>
        /// <param name="translatorId">Translator which made the update</param>
        /// <param name="newStatus">New status</param>
        void UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus);
    }
}
