using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Persistence.Contexts;
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
		}

		public async Task<int> CommitAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
