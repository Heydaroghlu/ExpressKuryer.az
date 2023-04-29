using ExpressKuryer.Application.Repositories;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Persistence.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{

		readonly DataContext _context;
		public Repository(DataContext context)
		{
			_context = context;
		}

		DbSet<TEntity> Table
		{
			get => _context.Set<TEntity>();
			set => _context.Set<TEntity>();
		}

		public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var query = Table.Where(expression);
			IsTracking(query, tracking);
			Includes(query,includes);
			return query.ToList();
		}

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var query = Table.AsQueryable<TEntity>();
			IsTracking(query, tracking);
			Includes(query, includes);
			return await query.FirstOrDefaultAsync(expression);
		}

		public async Task InsertAsync(TEntity entity)
		{
			await Table.AddAsync(entity);
		}

		public async Task InsertRangeAsync(List<TEntity> entities)
		{
			await Table.AddRangeAsync(entities);
		}

		public async Task<bool> IsAny(Expression<Func<TEntity, bool>> expression)
		{
		    return await Table.AnyAsync(expression);
		}

		public async Task Remove(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var entity = await Table.FirstOrDefaultAsync(expression);
			Table.Remove(entity);
		}

		public async Task RemoveRange(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var entities = Table.Where(expression).ToList();
			Table.RemoveRange(entities);
		}

		private void IsTracking(IQueryable<TEntity> query, bool tracking)
		{
			if (!tracking)
			{
				query = query.AsNoTracking();
			}
		}
		private void Includes(IQueryable<TEntity> query, params string[] includes)
		{
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
		}

    }
}
