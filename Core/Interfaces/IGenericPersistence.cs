using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Auktion.Core.Interfaces
{
    public interface IGenericPersistence<TDb, TDomain>
        where TDb : class
        where TDomain : class 
    {
        /// <summary>
        /// Retrieves all entities with optional filtering and navigation property inclusion.
        /// </summary>
        /// <param name="filter">Filtering expression to apply.</param>
        /// <param name="includes">Navigation properties to include.</param>
        /// <returns>Collection of domain entities.</returns>
        Collection<TDomain> Get(
            Expression<Func<TDb, bool>> filter = null,
            params Expression<Func<TDb, object>>[] includes);

        /// <summary>
        /// Retrieves a single entity by ID with optional navigation property inclusion.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="includes">Navigation properties to include.</param>
        /// <returns>The domain entity.</returns>
        TDomain GetById(int id, params Expression<Func<TDb, object>>[] includes);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="domainEntity">The domain entity to create.</param>
        void Create(TDomain domainEntity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="domainEntity">The domain entity to update.</param>
        void Update(TDomain domainEntity);

        /// <summary>
        /// Removes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to remove.</param>
        void Remove(int id);
    }
}