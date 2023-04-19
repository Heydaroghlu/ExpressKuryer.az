using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{

		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
		Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
		Task<bool> IsAny(Expression<Func<TEntity, bool>> expression);


		Task InsertAsync(TEntity entity);
		Task InsertRangeAsync(List<TEntity> entities);
		
		Task Remove(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
		Task RemoveRange(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);


	}
}
