using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcsDigitalApi.Domain;
using EcsDigitalApi.Repositories;

namespace EcsDigitalApi.Services
{
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;

        public CarService(IMapper mapper, ICarRepository carRepository)
        {
            _mapper = mapper;
            _carRepository = carRepository;
        }

        public async Task<List<Car>> GetAll()
        {
            var cars = await _carRepository.GetAllCars();

            var mappedCars = cars.Select(car => _mapper.Map<Car>(car));

            return mappedCars.ToList();
        }

        public async Task<Car> Get(int carId)
        {
            var car = await _carRepository.GetCarById(carId);

            var mappedCars = _mapper.Map<Car>(car);

            return mappedCars;
        }

        public async Task<bool> Add(Car car)
        {
            var mappedCar = _mapper.Map<DbModels.Car>(car);

            return await _carRepository.AddCar(mappedCar);
        }

        public async Task<bool> Update(Car car)
        {
            var mappedCar = _mapper.Map<DbModels.Car>(car);

            return await _carRepository.UpdateCar(mappedCar);
        }

        public async Task<bool> Remove(int carId)
        {
            return await _carRepository.RemoveCar(carId);
        }
    }
}