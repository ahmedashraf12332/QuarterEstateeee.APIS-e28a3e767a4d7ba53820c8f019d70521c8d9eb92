using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Specifications
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public Expression<Func<TEntity, object>> OrderBY { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPageinationEnabled { get; set; }

    }
}
