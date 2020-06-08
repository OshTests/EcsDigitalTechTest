using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.ApiModels;

namespace EcsDigitalApi.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAll();
        Task<Car> Get(int carId);
        Task<bool> Add(Car car);
        Task<bool> Update(Car car);
        Task<bool> Remove(int carId);
    }
}