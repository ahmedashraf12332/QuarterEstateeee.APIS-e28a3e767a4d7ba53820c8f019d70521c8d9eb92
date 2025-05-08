using Microsoft.EntityFrameworkCore;
using Quarter.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory
{
    public class SpecificationsEvaluator<TEntity, TKey> where TEntity : Core.Entites.BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        {
            var query = inputQuery;
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBY is not null)
            {
                query = query.OrderBy(spec.OrderBY);
            }
            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPageinationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return query;
        }
    }
}
