using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory.Data.Configurations
{
    public class EstateLocationConfiguration : IEntityTypeConfiguration<EstateLocation>
    {
        public void Configure(EntityTypeBuilder<EstateLocation> builder)
        {
            builder.Property(e => e.City)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Area)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
