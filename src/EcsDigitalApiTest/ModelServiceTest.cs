using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using EcsDigitalApi.DbModels;
using EcsDigitalApi.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace EcsDigitalApiTest
{
    public class ModelServiceTest
    {

        [Fact]
        public async Task GetAll_NoCondition()
        {
            var newModel1 = GetNewModel("name 1");
            var newModel2 = GetNewModel("name 2");
            var modelService = await DependenciesTestHelper.GetModelServiceWithModels(GetName(nameof(GetAll_NoCondition)), newModel1, newModel2);

            var response = await modelService.GetAll().ConfigureAwait(false);

            response.Should().OnlyContain(model => ModelsEqual(model, newModel1) || ModelsEqual(model, newModel2));
        }

        [Fact]
        public async Task Get_NoCondition()
        {
            var newModel1 = GetNewModel("name 1");
            var modelService = await DependenciesTestHelper.GetModelServiceWithModels(GetName(nameof(Get_NoCondition)), newModel1);

            var response = await modelService.Get(1).ConfigureAwait(false);

            response.Should().BeEquivalentTo(newModel1, options =>
                options.Including(model => model.Name).Including(model => model.MakerId));
        }

        [Fact]
        public async Task Add_NoCondition()
        {
            var carsContext = await DependenciesTestHelper.GetInMemoryCarsContext(GetName(nameof(Add_NoCondition)));

            var expectedRelatedWords = new List<string> { "related", "words" };
            var relatedWordsService = new Mock<IRelatedWordsService>();
            relatedWordsService.Setup(p => p.GetRelatedWords(It.IsAny<string>()))
                .ReturnsAsync(() => expectedRelatedWords);

            var newModel1 = GetNewApiModel("name 1", 1);
            var modelService = await DependenciesTestHelper.GetModelServiceWithMakers(carsContext,
                relatedWordsService.Object, new Maker {Name = "Maker Name"});

            var added = await modelService.Add(newModel1).ConfigureAwait(false);
            var newModel = await modelService.Get(1).ConfigureAwait(false);

            added.Should().BeTrue("Model was not added to the DB");
            newModel.Should().BeEquivalentTo(newModel1, options =>
                options.Including(model => model.Name).Including(model => model.MakerId),
                "Added model was not found in the DB");

            var carRepository = await DependenciesTestHelper.GetInMemoryCarRepository(carsContext);
            var carAdded = await carRepository.AddCar(new Car{ ModelId = 1, Colour = Color.Aqua, Year = 1999 });
            var newCar = await modelService.Get(1).ConfigureAwait(false);

            carAdded.Should().BeTrue("Car was not added with this model to the DB");
            newCar.Should().NotBeNull("Car was not found in the DB");

            var expectedRelatedWordsString = expectedRelatedWords.Aggregate((p1, p2) => p1 + ", " + p2);

            newCar.RelatedWords.Should().Be(expectedRelatedWordsString);
        }

        private string GetName(string methodName)
        {
            return nameof(ModelServiceTest) + methodName;
        }

        [Fact]
        public async Task Remove_NoCondition()
        {
            var newModel1 = GetNewModel("name 1");
            var modelService = await DependenciesTestHelper.GetModelServiceWithModels(GetName(nameof(Remove_NoCondition)), newModel1);

            var removed = await modelService.Remove(1).ConfigureAwait(false);
            var removedModel = await modelService.Get(1).ConfigureAwait(false);

            removed.Should().BeTrue("Model was not removed in the DB");
            removedModel.Should().BeNull();
        }

        private static bool ModelsEqual(EcsDigitalApi.Domain.Model model, Model newModel1)
        {
            return model.MakerId == newModel1.MakerId && model.Name == newModel1.Name;
        }

        private Model GetNewModel(string name)
        {
            return new Model { Name = name };
        }

        private EcsDigitalApi.Domain.Model GetNewApiModel(string name, int makerId)
        {
            return new EcsDigitalApi.Domain.Model { Name = name, MakerId = makerId };
        }

    }
}
