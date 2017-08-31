using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class HelpSGFContext : DbContext
    {
        public HelpSGFContext(DbContextOptions<HelpSGFContext> options) : base(options) {}

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Entity_To_Tag> Entities_To_Tags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagType> TagTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //IF we want to go to single named tables
            //modelBuilder.Entity<Entity>().ToTable("Entity");
            //modelBuilder.Entity<Contact>().ToTable("Contact");
            //modelBuilder.Entity<Tag>().ToTable("Tag");
            //modelBuilder.Entity<TagType>().ToTable("TagType");

            //This is a multi column key
            modelBuilder.Entity<Entity_To_Tag>()
                .HasKey(K => new { K.EntityID, K.TagID });
        }
    }
}