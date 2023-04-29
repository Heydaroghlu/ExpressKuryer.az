using ExpressKuryer.Application.Abstractions.Token;
using ExpressKuryer.Application.Enums;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Infrastructure.Services.Email;
using ExpressKuryer.Infrastructure.Services.Token;
using ExpressKuryer.Infrastructure.Storages.CloudinaryStorages;
using ExpressKuryer.Infrastructure.Storages.LocalStorages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure
{
	public static class ServiceRegistration
	{

		public static void AddInfrastructureServices(this IServiceCollection services)
		{
		}
		public static void AddInfrastructureServices(this IServiceCollection services, StorageEnum storageEnum)
		{
			services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<EmailService>();


            switch (storageEnum)
			{
				case StorageEnum.LocalStorage:
					services.AddScoped<IStorage, LocalStorage>();
					break;
				case StorageEnum.CloudinaryStorage:
					services.AddScoped<IStorage, CloudinaryStorage>();
					break;
				default:
					services.AddScoped<IStorage, LocalStorage>();
					break;
			}
		}

	}
}
