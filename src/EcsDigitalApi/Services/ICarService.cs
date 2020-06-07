using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Models;

namespace EcsDigitalApi.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAll();
    }
}