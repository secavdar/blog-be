using Blog.Domain.Contract;
using Blog.Domain.Model;
using Blog.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Blog.Business.Base
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork()
        {
            _context = new BlogContext();

            _context.Configuration.ProxyCreationEnabled = false;
            _context.Configuration.LazyLoadingEnabled = false;
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            _context.Configuration.ValidateOnSaveEnabled = false;

            _repositories = new HashSet<object>();
        }

        private BlogContext _context;
        private HashSet<object> _repositories;
        private DbContextTransaction _transaction;

        public void Begin()
        {
            _transaction = _context.Database.BeginTransaction();
        }
        public bool Commit()
        {
            bool isSuccess = false;

            try
            {
                _context.SaveChanges();

                if (_transaction != null)
                    _transaction.Commit();

                isSuccess = true;
            }
            catch (DbEntityValidationException ex)
            {
                if (_transaction != null)
                    _transaction.Rollback();

                var errors = new HashSet<string>();

                foreach (var item in ex.EntityValidationErrors)
                    foreach (var item2 in item.ValidationErrors)
                        errors.Add(item2.PropertyName + " : " + item2.ErrorMessage);

                throw new Exception(String.Join(" // ", errors));
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    _transaction.Rollback();

                while (ex.InnerException != null)
                    ex = ex.InnerException;

                if (!String.IsNullOrEmpty(ex.Message) && ex.Message.Contains("duplicate"))
                    throw new Exception("Aynı kaydı birden fazla kez oluşturamazsınız.");
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                if (_transaction != null)
                    _transaction.Dispose();
            }

            return isSuccess;
        }

        public BaseRepository<T> Call<T>()
            where T : class, IEntity, new()
        {
            BaseRepository<T> repository = null;

            foreach (var item in _repositories)
            {
                repository = item as BaseRepository<T>;
                if (repository != null)
                    break;
            }

            if (repository == null)
            {
                repository = new BaseRepository<T>(_context);
                _repositories.Add(repository);
            }

            return repository;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}