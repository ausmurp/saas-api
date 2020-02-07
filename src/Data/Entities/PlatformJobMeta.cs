using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    [Table(nameof(PlatformJobMeta))]
    public class PlatformJobMeta : Model<int>
    {
        [MaxLength(500)]
        public string Name { get; set; }

        public PlatformJob Job { get; set; }

        [ForeignKey(nameof(Job))]
        public int JobId { get; set; }

        public string Value { get; set; }
    }
}
