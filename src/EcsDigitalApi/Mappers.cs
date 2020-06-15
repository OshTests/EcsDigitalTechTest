using AutoMapper;
using EcsDigitalApi.DbModels;

namespace EcsDigitalApi
{
    public class Mappers
    {
        public static void CreateMaps(IMapperConfigurationExpression config)
        {
            config.CreateMap<Car, Domain.Car>()
                .ForMember(car => car.Maker, expression => expression.MapFrom(car => car.Model.Maker.Name))
                .ForMember(car => car.Model, expression => expression.MapFrom(car => car.Model.Name))
                .ForMember(car => car.RelatedWords, expression => expression.MapFrom(car => car.Model.RelatedWords))
                .ForMember(car => car.Colour, expression => expression.MapFrom(car => car.Colour.Name));
            config.CreateMap<Domain.Car, Car>()
                .ForMember(model => model.Model, expression => expression.Ignore());

            config.CreateMap<Maker, Domain.Maker>();
            config.CreateMap<Domain.Maker, Maker>()
                .ForMember(model => model.Models, expression => expression.Ignore());

            config.CreateMap<Model, Domain.Model>()
                .ForMember(p => p.MakerName, expression => expression.MapFrom(model => model.Maker.Name));
            config.CreateMap<Domain.Model, Model>()
                .ForMember(model => model.Maker, expression => expression.Ignore())
                .ForMember(model => model.RelatedWords, expression => expression.Ignore());
        }
    }
}