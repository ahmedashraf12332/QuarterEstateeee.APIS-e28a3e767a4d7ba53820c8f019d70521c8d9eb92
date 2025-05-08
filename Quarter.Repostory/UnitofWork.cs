using Quarter.Core;
using Quarter.Core.Entites;
using Quarter.Core.Repositories.Contract;
using Quarter.Repostory.Data.Context;
using Quarter.Repostory.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly QuarterDbContexts _context;
        private readonly Dictionary<string, object> _repositories;

        public UnitOfWork(QuarterDbContexts context)
        {
            _context = context; // ✅ تصحيح تمرير الـ context
            _repositories = new Dictionary<string, object>(); // ✅ تصحيح نوع المتغير
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(type, repository);
            }

            return (IGenericRepository<TEntity, TKey>)_repositories[type]; // ✅ استخدام تحويل صريح
        }
    }
}
