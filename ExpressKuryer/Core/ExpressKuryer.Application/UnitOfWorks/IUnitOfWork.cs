using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.UnitOfWorks
{
	public interface IUnitOfWork
	{
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

        Task<int> CommitAsync();
	}
}
