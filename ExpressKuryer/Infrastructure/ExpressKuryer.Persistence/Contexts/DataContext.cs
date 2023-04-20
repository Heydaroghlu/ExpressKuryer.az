﻿using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.Contexts
{
	public class DataContext : IdentityDbContext
	{
		public DataContext(DbContextOptions<DataContext> options):base(options) { }
	 public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Delivery> Delivery { get; set; }
		public DbSet<Paket> Pakets { get; set; }
		public DbSet<Partner> Partners { get; set; }
		public DbSet<PartnerProduct> PartnerProducts { get; set; }
		public DbSet<ProductImages> ProductImages { get;set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Subscribe> Subscribes { get; set; }
		public DbSet<Vacancy> Vacancies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }

}
