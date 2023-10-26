using System;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Models;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.DAO
{
    public interface ITranslatorsDao
    {
        /// <summary>
        /// Retrieves all translators
        /// </summary>
        /// <returns>List of translators</returns>
        List<TranslatorModel> GetAllTranslators();

        /// <summary>
        /// Returns all translators with a given name
        /// </summary>
        /// <returns></returns>
        List<TranslatorModel> GetTranslatorsByName(string name);

        /// <summary>
        /// Creates new translator with a given parameter
        /// </summary>
        /// <param name="translator"></param>
        /// <returns>Newly created translator</returns>
        TranslatorModel CreateTranslator(TranslatorModel translator);

        /// <summary>
        /// Updates translator's status
        /// </summary>
        /// <param name="translatorId"></param>
        /// <param name="translatorStatus"></param>
        /// <exception cref="System.InvalidOperationException">When the translator with a given id doesn't exists</exception>
        void UpdateTranslatorStatus(int translatorId, TranslatorStatus translatorStatus);
    }
}
