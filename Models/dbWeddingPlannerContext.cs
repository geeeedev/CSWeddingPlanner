

using Microsoft.EntityFrameworkCore;

namespace CSWeddingPlanner.Models
{
    public class dbWeddingPlannerContext : DbContext
    {
        public dbWeddingPlannerContext(DbContextOptions options) : base(options) {}
        
        public DbSet<User> Users { get; set; }           
        public DbSet<Event> Events { get; set; } 
        public DbSet<RSVP> RSVPs { get; set; }
        // !!! DbSet NAME must match MySQL db table NAME; one DbSet per table
        // !!! Must have before running ef migrations
    }
}