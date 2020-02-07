using System;

namespace SaaSApi.Data.Framework
{
    public interface IPersistedModel : ITrackedModel
    {
        /// <summary>The Id of the user that deleted the record.</summary>
        int? DeletedById { get; set; }

        /// <summary>The date that the record was deleted.</summary>
        DateTime? DeletedUtc { get; set; }
    }

    public interface IPersistedModel<TId> : ITrackedModel<TId>, IPersistedModel
    {
    }

    public abstract class PersistedModel<TId> : TrackedModel<TId>, IPersistedModel<TId>
    {
        /// <summary>The Id of the user that deleted the record.</summary>
        public int? DeletedById { get; set; }

        /// <summary>The date that the record was deleted.</summary>
        public DateTime? DeletedUtc { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public bool IsDeleted => DeletedUtc.HasValue;
    }

}