using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EcsDigitalApi.Domain
{
    public class Car
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ModelId { get; set; }

        public string Model { get; set; }

        public string Maker { get; set; }

        public string RelatedWords { get; set; }

        [Required]
        [Range(1900, 2022)]
        public int Year { get; set; }

        [Required]
        public string Colour { get; set; }
    }
}