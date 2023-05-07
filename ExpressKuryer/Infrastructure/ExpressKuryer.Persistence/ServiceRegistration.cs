using AutoMapper.Internal;
using ExpressKuryer.Application.Abstractions.Token;
using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Persistence.Configurations;
using ExpressKuryer.Persistence.Contexts;
using ExpressKuryer.Persistence.Repositories;
using ExpressKuryer.Persistence.Services;
using ExpressKuryer.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

			services.AddScoped<LayoutService>();

            services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(ServiceConfiguration.ConnectionString);
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddIdentity<AppUser,IdentityRole>(opt=>
			{
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireUppercase = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();
		}
		
	}
}
