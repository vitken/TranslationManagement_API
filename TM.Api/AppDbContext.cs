﻿using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TranslationJobModel> TranslationJobs { get; set; }
        public DbSet<TranslatorModel> Translators { get; set; }
    }
}