using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    public class Account : PersistedModel<int>
    {
        public Account()
        {
            ConnectedPlatforms = new List<ConnectedPlatform>();
        }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(500)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string Phone { get; set; }
        public ICollection<ConnectedPlatform> ConnectedPlatforms { get; set; }
    }
}
