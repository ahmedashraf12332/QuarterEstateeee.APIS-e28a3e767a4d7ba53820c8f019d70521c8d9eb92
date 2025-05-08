using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Quarter.Repostory.Data.Configurations
{
    public class EstateConfigurations : IEntityTypeConfiguration<Estate>
    {
        public void Configure(EntityTypeBuilder<Estate> builder)
        {
            builder.Property(P => P.Name).HasMaxLength(200).IsRequired();
            builder.Property(P => P.Description).HasMaxLength(1000).IsRequired();
            builder.Property(P => P.Images).HasMaxLength(2000).IsRequired();
            builder.Property(P => P.Name).HasMaxLength(200).IsRequired(true);
            
            builder.Property(P => P.SquareMeters).IsRequired(true);
            builder.Property(P => P.NumOfBedrooms).IsRequired(true);
            builder.Property(P => P.NumOfBathrooms).IsRequired(true);
            builder.Property(P => P.NumOfFloor).IsRequired(true);
            builder.Property(P => P.Price).IsRequired();
            builder.HasOne(e => e.EstateType)
                .WithMany()  
                .HasForeignKey(e => e.EstateTypeId)
                .OnDelete(DeleteBehavior.Restrict); //هنا مش اينفع امحه لو مرتبط بعقار
            builder.HasOne(e => e.EstateLocation)
               .WithMany() 
               .HasForeignKey(e => e.EstateLocationId)
               .OnDelete(DeleteBehavior.Restrict);







        }
    }
}
