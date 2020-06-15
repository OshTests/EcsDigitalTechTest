using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Domain;

namespace EcsDigitalApi.Services
{
    public interface IMakerService
    {
        Task<List<Maker>> GetAll();
        Task<Maker> Get(int makerId);
        Task<bool> Add(Maker maker);
        Task<bool> Remove(int makerId);
    }
}