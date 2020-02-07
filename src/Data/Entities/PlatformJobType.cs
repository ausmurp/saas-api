using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    [Table(nameof(PlatformJobType))]
    public class PlatformJobType : LookupModel<int>
    {
    }
}
