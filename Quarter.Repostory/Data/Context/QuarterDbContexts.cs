using Microsoft.EntityFrameworkCore;
using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory.Data.Context
{
    public class QuarterDbContexts : DbContext
    {
        public QuarterDbContexts(DbContextOptions<QuarterDbContexts> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations if you have IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // إعدادات الفيفوريت
            modelBuilder.Entity<UserFavoriteEstate>()
                .HasIndex(f => new { f.UserId, f.EstateId })
                .IsUnique();

            modelBuilder.Entity<UserFavoriteEstate>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteEstates)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFavoriteEstate>()
                .HasOne(f => f.Estate)
                .WithMany(e => e.FavoritedBy)
                .HasForeignKey(f => f.EstateId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Estate> Estates { get; set; }
        public DbSet<EstateLocation> EstateLocations { get; set; }

        public DbSet<EstateType> EstateTypes { get; set; }
        public DbSet<UserFavoriteEstate> UserFavoriteEstates { get; set; }

    }
}