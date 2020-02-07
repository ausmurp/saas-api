using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    [Table(nameof(PlatformJob))]
    public class PlatformJob : TrackedModel<int>
    {
        public PlatformJob()
        {
            Meta = new List<PlatformJobMeta>();
        }

        [MaxLength(500)]
        public string Message { get; set; }

        public int Progress { get; set; }

        public ICollection<PlatformJobMeta> Meta { get; set; }

        public PlatformJobStatus Status { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }

        public PlatformJobType Type { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public ConnectedPlatform ConnectedPlatform { get; set; }

        [ForeignKey(nameof(ConnectedPlatform))]
        public int ConnectedPlatformId { get; set; }
    }
}
