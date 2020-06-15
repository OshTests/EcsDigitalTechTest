using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Domain;

namespace EcsDigitalApi.Services
{
    public interface IModelService
    {
        Task<List<Model>> GetAll();
        Task<Model> Get(int modelId);
        Task<bool> Add(Model model);
        Task<bool> Remove(int modelId);
    }
}