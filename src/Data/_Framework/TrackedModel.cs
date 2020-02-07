using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaSApi.Data.Framework
{
    public interface ITrackedModel : IModel
    {
        /// <summary>The user's Id that created this record.</summary>
        int? CreatedById { get; set; }

        /// <summary>The date that this record was created.</summary>
        DateTime CreatedUtc { get; set; }

        /// <summary>The Id of the user that modified this record.</summary>
        int? ModifiedById { get; set; }

        /// <summary>The date that this record was modified.</summary>
        DateTime? ModifiedUtc { get; set; }
    }

    public interface ITrackedModel<TId> : IModel<TId>, ITrackedModel
    {
    }

    public abstract class TrackedModel<TId> : Model<TId>, ITrackedModel<TId>
    {
        /// <summary>The user's Id that created this record.</summary>
        public int? CreatedById { get; set; }

        /// <summary>The date that this record was created.</summary>
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedUtc { get; set; }

        /// <summary>The Id of the user that modified this record.</summary>
        public int? ModifiedById { get; set; }

        /// <summary>The date that this record was modified.</summary>
        [Column(TypeName = "DateTime2")]
        public DateTime? ModifiedUtc { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public bool IsModified => ModifiedUtc.HasValue;

    }
}