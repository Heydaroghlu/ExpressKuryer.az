using AutoMapper;
using ExpressKuryer.Application.Profiles;
using ExpressKuryer.Application.Validators.ContactValidators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application
{
	public static class ServiceRegistration
	{

		public static void AddApplicationServices(this IServiceCollection services)
		{
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MapperProfile());
			});

			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);

			services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(ContactDtoValidator)));
        }
	}
}
