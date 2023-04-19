using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Storages
{
	public interface IStorage
	{
		void Upload();
		void Delete();
	}
}
