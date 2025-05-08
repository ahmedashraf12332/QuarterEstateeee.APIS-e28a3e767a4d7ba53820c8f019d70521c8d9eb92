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
    internal class EstateTypeConfiguration : IEntityTypeConfiguration<EstateType>
    {
        public void Configure(EntityTypeBuilder<EstateType> builder)
        {
            builder.Property(P => P.Name).HasMaxLength(200).IsRequired();
        }
    }
}
