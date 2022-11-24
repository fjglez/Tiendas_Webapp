using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practica.TiendasAPI.Entities
{
    public class Tienda
    {
        //public Tienda(int id, string name, string? description, ICollection<InterestPoint> interestPoints)
        //{
        //    Id = id;
        //    Name = name;
        //    Description = description;
        //    InterestPoints = interestPoints;
        //}
        //public Tienda(string name)
        //{
        //    Name = name;
        //}

        //public Tienda(int id, string name, string description)
        //{
        //    Id = id;
        //    Name = name;
        //    Description = description;
        //}

        //public Tienda(int id, string name)
        //{
        //    Id = id;
        //    Name = name;
        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? Description { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
