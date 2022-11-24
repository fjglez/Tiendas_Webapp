using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Practica.TiendasAPI.Models
{
    public class ProductoDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }

    }
}
