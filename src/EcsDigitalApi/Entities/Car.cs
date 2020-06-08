using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EcsDigitalApi.Entities
{
    public class Car
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ModelId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public Color Colour { get; set; }
    }
}