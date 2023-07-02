using ExpressKuryer.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.Configurations
{
	public class ServiceConfiguration
	{

		public static string ConnectionString
		{
			get
			{
				ConfigurationManager configurationManager = new ConfigurationManager();
				configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
			
				configurationManager.AddJsonFile("appsettings.json");

				return configurationManager.GetConnectionString("DataContext");
			}
		}

	}
}
