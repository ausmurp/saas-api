using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SaaSApi.Common;
using X.PagedList;

namespace SaaSApi.Data.Framework
{
    public interface IRepository<TContext>
    {
        TContext DbContext { get; }
    }

    public interface IRepository<T, TId> where T : IModel<TId>
    {
        Task<T> Add(int currentUserId, T model);

        Task<IEnumerable<T>> AddRange(int currentUserId, IEnumerable<T> models);

        Task DeleteById(int currentUserId, TId id);

        Task<IPagedList<T>> Get(int currentUserId, QueryParams queryParams = null);

        Task<T> GetById(int currentUserId, TId id, bool includeDeleted = false);

        Task Save(int currentUserId);
    }
}