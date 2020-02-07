using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace SaaSApi.Common
{
    /// <summary>Query parameters.</summary>
    [DataContract]
    public class QueryParams
    {
        [FromQuery(Name = "createdMax")]
        [DataMember]
        public DateTime? CreatedMax { get; set; }

        [FromQuery(Name = "createdMin")]
        [DataMember]
        public DateTime? CreatedMin { get; set; }

        [FromQuery(Name = "orderBy")]
        [DataMember]
        [MaxLength(500)]
        public string OrderBy { get; set; }

        [FromQuery(Name = "page")]
        [DataMember]
        public int? Page { get; set; }

        [FromQuery(Name = "limit")]
        [DataMember]
        public int? Limit { get; set; }

        [FromQuery(Name = "query")]
        [DataMember]
        [MaxLength(500)]
        public string Query { get; set; }

        [FromQuery(Name = "updatedMax")]
        [DataMember]
        public DateTime? UpdatedMax { get; set; }

        [FromQuery(Name = "updatedMin")]
        [DataMember]
        public DateTime? UpdatedMin { get; set; }

    }
}
