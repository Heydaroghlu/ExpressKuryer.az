using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Mozilla;
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
		public DbSet<Courier> Couriers { get; set; }
		public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<BeCouirer> BeCouirers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entiteis = ChangeTracker.Entries<BaseEntity>();

            foreach (var item in entiteis)
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
