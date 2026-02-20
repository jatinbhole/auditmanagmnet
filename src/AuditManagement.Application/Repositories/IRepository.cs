using System.Linq.Expressions;
using AuditManagement.Domain.Entities;

namespace AuditManagement.Application.Repositories;

/// <summary>
/// Generic repository interface for data access
/// </summary>
public interface IRepository<T> where T : AuditEntity
{
    /// <summary>
    /// Get entity by ID
    /// </summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get entities by predicate
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get single entity by predicate
    /// </summary>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Add entity
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Update entity
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Delete entity
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Delete entity by ID
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Check if entity exists
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get count
    /// </summary>
    Task<int> CountAsync();

    /// <summary>
    /// Save changes
    /// </summary>
    Task<int> SaveChangesAsync();
}
