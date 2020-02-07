using System.ComponentModel.DataAnnotations;

namespace SaaSApi.Data.Framework
{

    public interface ILookupModel : IModel
    {
        string Name { get; set; }
        string Description { get; set; }
    }

    public interface ILookupModel<TId> : IModel<TId>, ILookupModel
    {
    }

    public abstract class LookupModel<TId> : Model<TId>, ILookupModel<TId>
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }

}