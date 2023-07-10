namespace api.Modal
{
    public class User
    {
        public int Id { get; set; } // Primary key

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Organization { get; set; }
        
        public int ContactDetails { get; set; }

        public ICollection<ResearchBook> ResearchBooks { get; set; }

        public ICollection<ChatInteraction> ChatInteraction { get; set; }

        public ICollection<SearchQuery> SearchQuery { get; set; }

    }
}
