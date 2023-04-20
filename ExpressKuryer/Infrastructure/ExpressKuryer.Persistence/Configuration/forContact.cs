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
    public class forContact : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("express_contact");
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(25);
            builder.Property(x=>x.Phone).IsRequired().HasMaxLength(20);
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(35);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(300);

        }
    }
}
