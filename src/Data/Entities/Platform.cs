using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    public class Platform : LookupModel<int>
    {
        public Platform()
        {
            Variables = new List<PlatformVariable>();
        }
        public string ImageUrl { get; set; }

        public PlatformType Type { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }


        public ICollection<PlatformVariable> Variables { get; set; }
    }
}
