using Microsoft.EntityFrameworkCore;
using onion_architecture.Domain.Base;
using onion_architecture.Domain.Repositories;
using onion_architecture.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly onion_architecture_Context _context;
        DbSet<T> _dbSet;
        DateTime now = DateTime.Now;
        public GenericRepository(onion_architecture_Context context)
        {
            _context = context;
            _dbSet=_context.Set<T>();
        }
        public async Task<bool> CreateAsync(T entity)
        {
            if (!_dbSet.Any(e => e == entity))
            {
                entity.createdAt = DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second); ;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            entity.deletedAt= DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second);
            //_dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(x => x.deletedAt != null || x.deletedAt == DateTime.MinValue).ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            var entity=await _dbSet.FindAsync(id);
            return entity;
        }

        public IQueryable<T> Queryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var entry =_context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                var existingEntity = _dbSet.Find(GetKeyValues(entity).ToArray());
                if (existingEntity == null)
                {
                    return false;
                }
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return true;

        }
        private IEnumerable<object> GetKeyValues(T entity)
        {
            var keyProperties = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            foreach (var property in keyProperties)
            {
                yield return property.PropertyInfo.GetValue(entity);
            }
        }
    }
}
