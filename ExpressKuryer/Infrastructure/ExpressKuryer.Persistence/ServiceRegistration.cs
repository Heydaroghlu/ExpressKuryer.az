using AutoMapper.Internal;
using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Persistence.Configurations;
using ExpressKuryer.Persistence.Contexts;
using ExpressKuryer.Persistence.Repositories;
using ExpressKuryer.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence
{
	public static class ServiceRegistration 
	{
		public static void AddPersistenceServices(this IServiceCollection services)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(ServiceConfiguration.ConnectionString);
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}
		
	}
}
