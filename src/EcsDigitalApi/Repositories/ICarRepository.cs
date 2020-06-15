using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.DbModels;

namespace EcsDigitalApi.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars();
        Task<Car> GetCarById(int carId);
        Task<bool> UpdateCar(Car car);
        Task<bool> AddCar(Car car);
        Task<bool> RemoveCar(int carId);


        Task<IEnumerable<Maker>> GetAllMakers();
        Task<Maker> GetMakerById(int makerId);
        Task<bool> AddMaker(Maker maker);
        Task<bool> RemoveMaker(int makerId);

        Task<IEnumerable<Model>> GetAllModels();
        Task<Model> GetModelById(int modelId);
        Task<bool> AddModel(Model model);
        Task<bool> RemoveModel(int modelId);
    }
}