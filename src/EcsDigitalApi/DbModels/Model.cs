namespace EcsDigitalApi.DbModels
{
    public class Model
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RelatedWords { get; set; }

        public int MakerId { get; set; }

        public Maker Maker { get; set; }
    }
}