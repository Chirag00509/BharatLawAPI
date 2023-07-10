namespace api.Modal
{
    public class ChatInteraction
    {
        public int Id { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key
        public string Message { get; set; }
        public DateTime DateTime { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
