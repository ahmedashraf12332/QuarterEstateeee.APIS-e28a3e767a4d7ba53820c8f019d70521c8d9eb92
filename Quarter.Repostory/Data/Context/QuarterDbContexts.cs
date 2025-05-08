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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Estate> Estates { get; set; }
        public DbSet<EstateLocation> EstateLocations { get; set; }

        public DbSet<EstateType> EstateTypes { get; set; }

    }
}