using Blog.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Blog.Domain.Repository
{
    public class BaseRepository<T> where T : class, IEntity, new()
    {
        private IObjectContextAdapter Adapter
        {
            get { return _context as IObjectContextAdapter; }
        }

        protected DbContext _context;
        protected DbSet<T> _set;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public virtual IQueryable<T> All()
        {
            return _set.AsQueryable();
        }
        public virtual List<T> List()
        {
            return _set.ToList();
        }
        public virtual List<T> List(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate).ToList();
        }
        public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }
        public virtual T Find(object id)
        {
            return _set.Find(id);
        }
        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }
        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _set.Any(predicate);
        }
        public virtual T Add(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            return entity;
        }
        public virtual void AddMultiple(IEnumerable<T> entities)
        {
            foreach (var item in entities)
                this.Add(item);
        }
        public virtual T Reload(T entity)
        {
            _context.Entry(entity).Reload();
            return entity;
        }
        public virtual void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }
        public virtual void Delete(object id)
        {
            T entity = Find(id);
            _set.Remove(entity);
        }
        public virtual void DeleteMany(Expression<Func<T, bool>> predicate)
        {
            var list = _set.Where(predicate).ToList();

            if (list.Count != 0)
                _set.RemoveRange(list);
        }
        public virtual void Truncate()
        {
            var type = typeof(T);

            var adapter = (IObjectContextAdapter)_context;
            var objectContext = adapter.ObjectContext;

            if (objectContext.MetadataWorkspace != null)
            {
                var entity = objectContext.MetadataWorkspace
                                          .GetItems<EntityContainer>(DataSpace.SSpace)
                                          .First()
                                          .BaseEntitySets
                                          .FirstOrDefault(x => x.Name == type.Name);

                _context.Database.ExecuteSqlCommand(String.Format("delete from {0}.{1}", entity.Schema, entity.Table));
                _context.Database.ExecuteSqlCommand(String.Format("dbcc checkident ([{0}.{1}], reseed, 0)", entity.Schema, entity.Table));
            }
        }
        public virtual void RemoveRelationship<P>(T parent, Expression<Func<T, object>> predicate, P child) where P : class, IEntity, new()
        {
            Adapter.ObjectContext
                   .ObjectStateManager
                   .ChangeRelationshipState(parent, child, predicate, EntityState.Deleted);
        }
        public virtual void AddRelationship<P>(T parent, Expression<Func<T, object>> predicate, P child) where P : class, IEntity, new()
        {
            Adapter.ObjectContext
                   .ObjectStateManager
                   .ChangeRelationshipState(parent, child, predicate, EntityState.Added);
        }
    }
}