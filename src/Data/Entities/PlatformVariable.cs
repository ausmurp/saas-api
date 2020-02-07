using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    /// <summary>This table contains the different credentials necessary to authenticate a platform.</summary>
    [Table(nameof(PlatformVariable))]
    public class PlatformVariable : TrackedModel<int>, ILookupModel
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Value { get; set; }

        [MaxLength(500)]
        public string RegexValidation { get; set; }

        [MaxLength(500)]
        public string Label { get; set; }

        public bool IsRequired { get; set; }

        public bool CanModify { get; set; }

        public Platform Platform { get; set; }

        [ForeignKey(nameof(Platform))]
        public int PlatformId { get; set; }
    }
}