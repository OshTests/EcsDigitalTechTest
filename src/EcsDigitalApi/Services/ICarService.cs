using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.ApiModels;

namespace EcsDigitalApi.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAll();
        Task<Car> Get(int id);
        Task Create(Car car);
        Task Update(Car car);
        Task Delete(int id);
    }
}