using System.ComponentModel.DataAnnotations;

namespace EcsDigitalApi.Domain
{
    public class Maker
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}