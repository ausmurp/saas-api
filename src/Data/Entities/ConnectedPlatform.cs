using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    public class ConnectedPlatform : PersistedModel<int>
    {
        public ConnectedPlatform()
        {
            Jobs = new List<PlatformJob>();
        }

        public Account Account { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        public Platform Platform { get; set; }

        [ForeignKey(nameof(Platform))]
        public int PlatformId { get; set; }

        public ICollection<PlatformJob> Jobs { get; set; }

    }
}
