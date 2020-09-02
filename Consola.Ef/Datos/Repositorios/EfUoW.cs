/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:51:30
 *
 */

using System;

using EfConsola.Modelo;


using Microsoft.EntityFrameworkCore.Storage;

namespace EfConsola.Datos.Repositorios
{
    /// <summary>
    /// EfUoW
    /// </summary>
    public class EfUoW : IUnitOfWork
    {

        private readonly MiDbContext _context;
        private readonly string _connString;
        private IDbContextTransaction _transaction=null;

        public EfUoW(string connString)
        {
            _connString = connString;
            _context = new MiDbContext(_connString);
        }



        private IAutorRepository _autorRepo;
        private IBlogRepository _blogRepos;

        //public TRepository GetRepository<TRepository>() where TRepository : IRepository
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
           Type type = typeof(TEntity);

            if (type == typeof(IAutorRepository))
            
                return _autorRepo ?? (_autorRepo = new EfAutorRepository(_context.Set<Autor>()));
            
            else if (type == typeof(IBlogRepository))
                return (TRepository)(IBlogRepository)new EfBlogRepository(_context.Set<Blog>());

            throw new NotImplementedException(type.Name + " No definido");
        }

        public void BeginTrans()
        {
            if ( _transaction!=null)
                throw new Exception("ya existe una transacción");

            _transaction = _context.Database.BeginTransaction();
        }

        public void Commint()
        {
            try
            {

                if (_transaction is null)
                    throw new Exception("Debe iniciar una transacción");

                _context.SaveChanges();


                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}