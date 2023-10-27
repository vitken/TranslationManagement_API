using System;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Models
{
    public class TranslatorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double HourlyRate { get; set; }
        public TranslatorStatus Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
