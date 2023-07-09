using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Persistence.Contexts;
using ExpressKuryer.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            RepositoryUser = new Repository<AppUser>(_context);
            RepositoryContact = new Repository<Contact>(_context);
            RepositoryDelivery = new Repository<Delivery>(_context);
            RepositoryPaket = new Repository<Paket>(_context);
            RepositoryPartner = new Repository<Partner>(_context);
            RepositoryPartnerProduct = new Repository<PartnerProduct>(_context);
            RepositoryService = new Repository<Service>(_context);
            RepositorySetting = new Repository<Setting>(_context);
            RepositorySlider = new Repository<Slider>(_context);
            RepositorySubscribe = new Repository<Subscribe>(_context);
            RepositoryVacancy = new Repository<Vacancy>(_context);
            RepositoryCourier = new Repository<Courier>(_context);
            RepositoryJobSeeker = new Repository<JobSeeker>(_context);
            RepositoryProductImages = new Repository<ProductImages>(_context);
            RepositoryBeCourier = new Repository<BeCouirer>(_context);
        }

        public IRepository<AppUser> RepositoryUser { get; set; }
        public IRepository<Contact> RepositoryContact { get; set; }
        public IRepository<Delivery> RepositoryDelivery { get; set; }
        public IRepository<Paket> RepositoryPaket { get; set; }
        public IRepository<Partner> RepositoryPartner { get; set; }
        public IRepository<PartnerProduct> RepositoryPartnerProduct { get; set; }
        public IRepository<Service> RepositoryService { get; set; }
        public IRepository<Setting> RepositorySetting { get; set; }
        public IRepository<Slider> RepositorySlider { get; set; }
        public IRepository<Subscribe> RepositorySubscribe { get; set; }
        public IRepository<Vacancy> RepositoryVacancy { get; set; }
        public IRepository<Courier> RepositoryCourier { get; set; }
        public IRepository<JobSeeker> RepositoryJobSeeker { get; set; }
        public IRepository<ProductImages> RepositoryProductImages { get; set; }
        public IRepository<BeCouirer> RepositoryBeCourier { get; set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
