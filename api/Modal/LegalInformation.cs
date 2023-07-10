namespace api.Modal
{
    public class LegalInformation
    {
        public int Id { get; set; } // Primary key
        public LegalInformationType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Document { get; set; } // byte array for storing document data
        public DateTime DateAdded { get; set; }
        public int ResearchBookId { get; set; } // Foreign key

        // Navigation property
        public ResearchBook ResearchBook { get; set; }

    }
    public enum LegalInformationType
    {
        ActAndSection,
        CourtAndProcedure,
        RelevantFacts,
        Statistics,
        SupportingMaterial,
        FullJudgment
    }
}
