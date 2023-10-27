using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.DAO;
using TranslationManagement.Api.Models;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Route("api/TranslatorsManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly ITranslatorsDao _translatorsDao;

        public TranslatorManagementController(ITranslatorsDao translatorsDao, ILogger<TranslatorManagementController> logger)
        {
            _translatorsDao = translatorsDao;
            _logger = logger;
        }

        [HttpGet]
        public List<TranslatorModel> GetTranslators()
        {
            return _translatorsDao.GetAllTranslators();
        }

        [HttpGet]
        public List<TranslatorModel> GetTranslatorsByName(string name)
        {
            return _translatorsDao.GetTranslatorsByName(name);
        }

        [HttpPost]
        public TranslatorModel AddTranslator(TranslatorModel translator)
        {
            return _translatorsDao.CreateTranslator(translator);
        }
        
        [HttpPost]
        public IActionResult UpdateTranslatorStatus(int translatorId, TranslatorStatus newStatus)
        {
            _logger.LogInformation($"User status update request: {Enum.GetName(typeof(TranslatorStatus), newStatus)} for user {translatorId}");
            try
            {
                _translatorsDao.UpdateTranslatorStatus(translatorId, newStatus);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound($"Translator with id {translatorId} not found!");
            }
            
            return Ok();
        }
    }
}