using System;
using System.Threading.Tasks;

namespace FundacionOlivar.DDD.SharedKernel
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes.
    /// A Unit of Work keeps track of everything you do
    /// during a business transaction that can affect the database.
    /// </summary>
    /// <see cref="https://martinfowler.com/eaaCatalog/unitOfWork.html"/>
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Marca el inicio de la operación trasaccional del UoW
        /// </summary>
        void Begin();

        /// <summary>
        /// Confirma la transacción
        /// </summary>
        void Commit();

        /// <summary>
        /// /// Confirma la transacción asincronamente
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        //Task<int> CommitAsync();

        //void Rollback();
    }
}