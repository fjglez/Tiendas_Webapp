using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Practica.TiendasAPI.Models
{
    public class TiendaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<ProductoDto> productos { get; set; } = new List<ProductoDto>();



    }
}
