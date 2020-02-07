using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    [Table(nameof(PlatformJobStatus))]
    public class PlatformJobStatus : LookupModel<int>
    {
    }
}
