using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaSApi.Data.Framework
{
    public interface IModel
    {
    }

    public interface IModel<TId> : IModel
    {
        TId Id { get; set; }
    }

    /// <summary>
    /// Base class for all entities.
    /// </summary>
    /// <remarks>
    /// This is an empty "marker" class used to tell models apart from other classes. 
    /// It is not likely to have any kind of base functionality in it.
    /// </remarks>
    public abstract class Model : IModel
    {
    }

    public abstract class Model<TId> : Model, IModel<TId>
    {
        /// <summary>The primary key / identity column.</summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TId Id { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public bool IsNew => Id.Equals(default(TId));
    }
}
