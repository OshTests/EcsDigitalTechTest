using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcsDigitalApi.Domain;
using EcsDigitalApi.Repositories;

namespace EcsDigitalApi.Services
{
    public class ModelService : IModelService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly IRelatedWordsService _relatedWordsService;

        public ModelService(IMapper mapper, ICarRepository carRepository, IRelatedWordsService relatedWordsService)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _relatedWordsService = relatedWordsService;
        }

        public async Task<List<Model>> GetAll()
        {
            var models = (await _carRepository.GetAllModels()).ToList();

            var mappedModels = models.Select(model => _mapper.Map<Model>(model)).ToList();

            return mappedModels;
        }

        public async Task<Model> Get(int modelId)
        {
            var model = await _carRepository.GetModelById(modelId);

            var mappedModel = _mapper.Map<Model>(model);

            return mappedModel;
        }

        public async Task<bool> Add(Model model)
        {
            var mappedModel = _mapper.Map<DbModels.Model>(model);

            var relatedWords = (await _relatedWordsService.GetRelatedWords(mappedModel.Name)).ToList();
            mappedModel.RelatedWords = relatedWords.Any()
                    ? relatedWords.Take(10).Aggregate((p1, p2) => p1 + ", " + p2) 
                    : string.Empty;

            return await _carRepository.AddModel(mappedModel);
        }

        public async Task<bool> Remove(int modelId)
        {
            return await _carRepository.RemoveModel(modelId);
        }
    }
}