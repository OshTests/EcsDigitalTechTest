using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Domain;

namespace EcsDigitalApi.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAll();
        Task<Car> Get(int carId);
        Task<bool> Add(Car car);
        Task<bool> Update(Car car);
        Task<bool> Remove(int carId);
    }
}