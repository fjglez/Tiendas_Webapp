using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Practica.TiendasAPI.Models
{
    public class ProductoUpdateDto
    {
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
