using api.Modal;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class LegalGenContext : DbContext 
    {
        public LegalGenContext(DbContextOptions<LegalGenContext> options ) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<LegalInformation>().ToTable("LegalInformation");
            modelBuilder.Entity<ResearchBook>().ToTable("ResearchBook");
            modelBuilder.Entity<SearchQuery>().ToTable("SearchQuery");
            modelBuilder.Entity<ChatInteraction>().ToTable("ChatInteraction");
        }

        public DbSet<api.Modal.User>? User { get; set; }

        public DbSet<api.Modal.LegalInformation>? LegalInformation { get; set; }

        public DbSet<api.Modal.ResearchBook>? ResearchBook { get; set; }

        public DbSet<api.Modal.SearchQuery>? SearchQuery { get; set; }

        public DbSet<api.Modal.ChatInteraction>? ChatInteraction { get; set; }

    }
}
