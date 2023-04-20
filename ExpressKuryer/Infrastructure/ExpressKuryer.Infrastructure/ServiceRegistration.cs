using ExpressKuryer.Application.Enums;
using ExpressKuryer.Application.Storages;
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
			switch (storageEnum)
			{
				case StorageEnum.LocalStorage:
					services.AddScoped<IStorage, LocalStorage>();
					break;
				case StorageEnum.CloudfareStorage:
					services.AddScoped<IStorage, CloudinaryStorage>();
					break;
				default:
					services.AddScoped<IStorage, LocalStorage>();
					break;
			}
		}

	}
}
