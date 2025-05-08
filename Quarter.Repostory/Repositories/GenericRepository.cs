using Microsoft.EntityFrameworkCore;
using Quarter.Core.Entites;
using Quarter.Core.Repositories.Contract;
using Quarter.Core.Specifications;
using Quarter.Repostory.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly QuarterDbContexts _context;

        public GenericRepository(QuarterDbContexts context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Estate))
            {
                return (IEnumerable<TEntity>)await _context.Estates
                    .OrderBy(p => p.Name)
                    .Include(p => p.EstateLocation)
                    .Include(p => p.EstateType)
                    .ToListAsync();
            }

            return await _context.Set<TEntity>().ToListAsync();
        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, Tkey> spec)
        {
            return SpecificationsEvaluator<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), spec);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Tkey id)
        {
            if (typeof(TEntity) == typeof(Estate))
            {
                return await _context.Estates.Where(p => p.Id.Equals(id))
                    .Include(p => p.EstateLocation)
                    .Include(p => p.EstateType)
                    .FirstOrDefaultAsync() as TEntity;
            }

            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
    }
}

