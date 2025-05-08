using Quarter.Core.Entites;
using Quarter.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core
{
    public interface IUnitofWork
    {
        Task<int> CompleteAsync();
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    }
}
