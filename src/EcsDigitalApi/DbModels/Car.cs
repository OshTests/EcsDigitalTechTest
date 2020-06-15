using System.Drawing;

namespace EcsDigitalApi.DbModels
{
    public class Car
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Year { get; set; }

        public Color Colour { get; set; }
    }
}