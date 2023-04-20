using ExpressKuryer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.Configuration
{
    public class forPatnerProduct : IEntityTypeConfiguration<PartnerProduct>
    {
        public void Configure(EntityTypeBuilder<PartnerProduct> builder)
        {
            builder.ToTable("express_PartnersProducts");
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.SalePrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DiscountPrice).HasColumnType("decimal(18,2)");


        }
    }
}
