using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EcsDigitalApi.ApiModels
{
    public class Car
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ModelId { get; set; }

        public string Model { get; set; }

        public string Maker { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public Color Colour { get; set; }
    }
}