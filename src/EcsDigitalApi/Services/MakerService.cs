using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcsDigitalApi.Domain;
using EcsDigitalApi.Repositories;

namespace EcsDigitalApi.Services
{
    public class MakerService : IMakerService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;

        public MakerService(IMapper mapper, ICarRepository carRepository)
        {
            _mapper = mapper;
            _carRepository = carRepository;
        }

        public async Task<List<Maker>> GetAll()
        {
            var makers = await _carRepository.GetAllMakers();

            var mappedMakers = makers.Select(maker => _mapper.Map<Maker>(maker)).ToList();

            return mappedMakers;
        }

        public async Task<Maker> Get(int makerId)
        {
            var maker = await _carRepository.GetMakerById(makerId);

            var mappedMaker = _mapper.Map<Maker>(maker);

            return mappedMaker;
        }

        public async Task<bool> Add(Maker maker)
        {
            var mappedMaker = _mapper.Map<DbModels.Maker>(maker);

            return await _carRepository.AddMaker(mappedMaker);
        }

        public async Task<bool> Remove(int makerId)
        {
            return await _carRepository.RemoveMaker(makerId);
        }
    }
}