using Quarter.Core.Entites;
using Quarter.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> spec);
        Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, Tkey> spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(); // ✅ الصح
        Task<TEntity> GetAsync(Tkey id); // ✅ الصح
        Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec);
    }
}
