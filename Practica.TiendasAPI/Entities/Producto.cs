using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Practica.TiendasAPI.Entities
{
    public class Producto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; } = 1.0;

        [ForeignKey("ShopId")]
        public Tienda? Shop { get; set; }
        public int ShopId { get; set; }

    }
}
