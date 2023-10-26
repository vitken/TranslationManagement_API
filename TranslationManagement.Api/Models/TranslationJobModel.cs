using System;
using static TranslationManagement.Api.Utils.CommonUtils;

namespace TranslationManagement.Api.Models
{
    public class TranslationJobModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public JobStatus Status { get; set; }
        public string OriginalContent { get; set; }
        public string TranslatedContent { get; set; }
        public double Price { get; set; }
    }
}
