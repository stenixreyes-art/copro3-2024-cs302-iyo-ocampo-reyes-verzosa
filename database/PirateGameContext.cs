using Microsoft.EntityFrameworkCore;

namespace Morgan_Thieves.database
{
    public class PirateGameContext(DbContextOptions<PirateGameContext> options) : DbContext(options)
    {
        public DbSet<CharacterEntity> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CharacterEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(14);
                entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.LastModified).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
