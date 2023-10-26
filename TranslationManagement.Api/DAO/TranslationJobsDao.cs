using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Models;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.DAO
{
    public class TranslationJobsDao : ITranslationJobsDao
    {
        private AppDbContext _context;

        public TranslationJobsDao(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetService<AppDbContext>();
        }

        public TranslationJobModel CreateJob(TranslationJobModel job)
        {
            var newJob = _context.TranslationJobs.Add(job);
            _context.SaveChanges();
            return newJob.Entity;
        }

        public List<TranslationJobModel> GetJobs()
        {
            return _context.TranslationJobs.ToList();
        }

        public void UpdateJobStatus(int jobId, JobStatus newStatus)
        {
            var job = _context.TranslationJobs.Single(j => j.Id == jobId);
            job.Status = newStatus;
            _context.SaveChanges();
        }
    }
}
