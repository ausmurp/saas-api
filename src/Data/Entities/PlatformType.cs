using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    [Table(nameof(PlatformType))]
    public class PlatformType : LookupModel<int>
    {
    }
}
