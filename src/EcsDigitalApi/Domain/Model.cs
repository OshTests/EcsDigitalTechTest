using System.ComponentModel.DataAnnotations;

namespace EcsDigitalApi.Domain
{
    public class Model
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int MakerId { get; set; }

        public string MakerName { get; set; }

        public string RelatedWords { get; set; }
    }
}