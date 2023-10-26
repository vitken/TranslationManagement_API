using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TranslationManagement.Api.Models;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.DAO
{
    public interface ITranslationJobsDao
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
        /// Updates job status
        /// </summary>
        /// <param name="jobId">The job to update</param>
        /// <param name="translatorId">Translator which made the update</param>
        /// <param name="newStatus">New status</param>
        void UpdateJobStatus(int jobId, JobStatus newStatus);
    }
}
