﻿using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Specifications
{
    public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBY { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPageinationEnabled { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;

        }
        public BaseSpecifications()
        {

        }
        public void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBY = expression;

        }
        public void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            OrderByDescending = expression;

        }
        public void ApplyPagination(int skip, int take)
        {
            IsPageinationEnabled = true;
            Skip = skip;
            Take = take;
        }

    }
}
