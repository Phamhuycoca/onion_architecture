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
            _dbSet = _context.Set<T>();
        }

        public bool Create(T entity)
        {
            if (!_dbSet.Any(e => e == entity))
            {
                entity.createdAt = DateTime.Now;
                _dbSet.Add(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(long id)
        {
            var entity = _dbSet.Find(id);

            if (entity == null || entity.IsDelete!=false)
            {
                return false;
            }

            entity.deletedAt = DateTime.Now;
            entity.IsDelete = true;
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return true;
        }

        public List<T> GetAll()
        {
            return _dbSet.Where(x=>x.IsDelete==false).OrderByDescending(x => x.createdAt).ToList();
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public bool Update(T entity)
        {
            var entry = _context.Entry(entity);

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
                _context.SaveChanges();
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
