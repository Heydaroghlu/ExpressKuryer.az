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
    public class forVacancy : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.ToTable("express_vacancies");
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Message).HasMaxLength(200);


        }
    }
}
