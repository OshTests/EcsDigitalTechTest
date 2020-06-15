using System.Collections.Generic;

namespace EcsDigitalApi.DbModels
{
    public class Maker
    {
        public Maker()
        {
            Models = new List<Model>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Model> Models { get; set; }
    }
}