using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Controllers;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Utils;

namespace TranslationManagement.Api.DAO
{
    public class TranslatorsDao : ITranslatorsDao
    {
        private AppDbContext _context;

        public TranslatorsDao(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetService<AppDbContext>();
        }

        public TranslatorModel CreateTranslator(TranslatorModel translator)
        {
            var newTranslator = _context.Translators.Add(translator);
            _context.SaveChanges();
            return newTranslator.Entity;
        }

        public List<TranslatorModel> GetAllTranslators()
        {
            return _context.Translators.ToList(); ;
        }

        public List<TranslatorModel> GetTranslatorsByName(string name)
        {
            return _context.Translators.Where(x => x.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToList();
        }

        public void UpdateTranslatorStatus(int translatorId, CommonUtils.TranslatorStatus translatorStatus)
        {
            var translator = _context.Translators.Single(j => j.Id == translatorId);
            translator.Status = translatorStatus;
            _context.SaveChanges();
        }
    }
}
