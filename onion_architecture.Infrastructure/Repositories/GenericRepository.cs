using Microsoft.EntityFrameworkCore;
using onion_architecture.Domain.Base;
using onion_architecture.Domain.Repositories;
using onion_architecture.Infrastructure.Context;
using onion_architecture.Infrastructure.Exceptions;
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

        public T Create(T entity)
        {
            if (!_dbSet.Any(e => e == entity))
            {
                entity.createdAt = DateTime.Now;
                _dbSet.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            return null;
        }

        public T Delete(long id)
        {
           
            try
            {
                var entity = _dbSet.Find(id);

                if (entity == null || entity.IsDelete != false)
                {
                    throw new ApiException(400, "Không tìm thấy thông tin");
                }

                entity.deletedAt = DateTime.Now;
                entity.IsDelete = true;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }

        public List<T> GetAll()
        {
            return _dbSet.Where(x => x.IsDelete == false).OrderByDescending(x => x.createdAt).ToList();
        }

        public T GetById(long id)
        {
            var item = _dbSet.Find(id);
            if (item == null || item.IsDelete)
            {
                throw new ApiException(404, "Không tìm thấy thông tin");
            }
            /*if (item.IsDelete)
            {
                throw new ApiException(404, "Không tìm thấy thông tin");
            }*/
            return item;
        }

        public T Update(T entity)
        {
            if (!_dbSet.Any(e => e == entity))
            {
                throw new ApiException(404, "Không tìm thấy thông tin");
            }
            entity.updatedAt = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
                return entity;

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
       /* public T Update(T entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var existingEntity = _dbSet.Find(GetKeyValues(entity).ToArray());

                if (existingEntity == null)
                {
                    throw new ApiException(404, "Không tìm thấy thông tin");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            try
            {
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }*/
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
