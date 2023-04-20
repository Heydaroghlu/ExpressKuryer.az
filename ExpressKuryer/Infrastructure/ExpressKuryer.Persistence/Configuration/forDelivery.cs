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
    public class forDelivery : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("express_deliveries");
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(25);
            builder.Property(x => x.Telephone).IsRequired().HasMaxLength(25);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(25);
            builder.Property(x => x.AddressFrom).IsRequired().HasMaxLength(150);
            builder.Property(x => x.AddressTo).IsRequired().HasMaxLength(150);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DisCount).HasColumnType("decimal(18,2)");



        }
    }
}
