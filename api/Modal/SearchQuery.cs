namespace api.Modal
{
    public class SearchQuery
    {
        public int Id { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key
        public string Keywords { get; set; }
        public DateTime DateTime { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
