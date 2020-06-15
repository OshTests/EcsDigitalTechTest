using System.Threading.Tasks;
using EcsDigitalApi.DbModels;
using FluentAssertions;
using Xunit;

namespace EcsDigitalApiTest
{
    public class MakerServiceTest
    {

        [Fact]
        public async Task GetAll_NoCondition()
        {
            var newMaker1 = GetNewMaker("name 1");
            var newMaker2 = GetNewMaker("name 2");
            var makerService = await DependenciesTestHelper.GetMakerServiceWithMakers(nameof(MakerServiceTest) + nameof(GetAll_NoCondition), newMaker1, newMaker2);

            var response = await makerService.GetAll().ConfigureAwait(false);

            response.Should().OnlyContain(maker => MakersEqual(maker, newMaker1) || MakersEqual(maker, newMaker2));
        }

        [Fact]
        public async Task Get_NoCondition()
        {
            var newMaker1 = GetNewMaker("name 1");
            var makerService = await DependenciesTestHelper.GetMakerServiceWithMakers(nameof(MakerServiceTest) + nameof(Get_NoCondition), newMaker1);

            var response = await makerService.Get(1).ConfigureAwait(false);

            response.Should().BeEquivalentTo(newMaker1, options => options.Including(maker => maker.Name));
        }

        [Fact]
        public async Task Add_NoCondition()
        {
            var newMaker1 = GetNewApiMaker("name 1");
            var makerService = await DependenciesTestHelper.GetMakerServiceWithMakers(nameof(MakerServiceTest) + nameof(Add_NoCondition));

            var added = await makerService.Add(newMaker1).ConfigureAwait(false);
            var newMaker = await makerService.Get(1).ConfigureAwait(false);

            added.Should().BeTrue("Maker was not added to the DB");
            newMaker.Should().BeEquivalentTo(newMaker1, options =>
                options.Including(maker => maker.Name), "Added maker was not found in the DB");
        }

        [Fact]
        public async Task Remove_NoCondition()
        {
            var newMaker1 = GetNewMaker("name 1");
            var makerService = await DependenciesTestHelper.GetMakerServiceWithMakers(nameof(MakerServiceTest) + nameof(Remove_NoCondition), newMaker1);

            var removed = await makerService.Remove(1).ConfigureAwait(false);
            var removedMaker = await makerService.Get(1).ConfigureAwait(false);

            removed.Should().BeTrue("Maker was not removed in the DB");
            removedMaker.Should().BeNull();
        }

        private static bool MakersEqual(EcsDigitalApi.Domain.Maker maker, Maker newMaker1)
        {
            return maker.Name == newMaker1.Name;
        }

        private Maker GetNewMaker(string name)
        {
            return new Maker { Name = name };
        }

        private EcsDigitalApi.Domain.Maker GetNewApiMaker(string name)
        {
            return new EcsDigitalApi.Domain.Maker { Name = name };
        }

    }
}
