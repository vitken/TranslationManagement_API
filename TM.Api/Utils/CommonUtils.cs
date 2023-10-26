namespace TranslationManagement.Api.Utils
{
    public static class CommonUtils
    {
        public enum JobStatus
        {
            New,
            Inprogress,
            Completed
        }

        public enum TranslatorStatus
        { 
            Applicant,
            Certified,
            Deleted 
        };

        public const double PricePerCharacter = 0.01;
    }
}
