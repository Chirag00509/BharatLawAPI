namespace api.Modal
{
    public class ResearchBook
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<LegalInformation> LegalInformation { get; set; }

    }
}
