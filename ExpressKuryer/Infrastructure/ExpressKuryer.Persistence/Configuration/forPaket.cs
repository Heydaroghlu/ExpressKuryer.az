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
    public class forPaket : IEntityTypeConfiguration<Paket>
    {
        public void Configure(EntityTypeBuilder<Paket> builder)
        {
            builder.ToTable("express_pakets");
            builder.Property(x=>x.Title).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(350);
            builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");



        }
    }
}
