using AutoMapper;
using Bestemmie_per_Igor;

//Test class to record mapping
#region
var configuration1 = new MapperConfiguration(
    cfg =>
        cfg.CreateMap<Source, Destination>()
        .ForCtorParam("MoreMemes",    //Breaks MapFrom?
                                      //.ForMember(src => src.MoreMemes,
        opt =>
            opt.MapFrom(src => PINGAS.cancer(src.Memes))
            )
        );
//configuration.AssertConfigurationIsValid();
var mapper = configuration1.CreateMapper();

var source = new Source
{
    Title = "A",
    Id = "C",
    Memes = "123456"
};

var result1 = mapper.Map<Source, Destination>(source);
#endregion

//Test mapping from nested classes to flat record
#region
var configuration2 = new MapperConfiguration(
    cfg =>
        cfg.CreateMap<In1, Out>()
        .ForCtorParam("MyTest",
        opt =>
            opt.MapFrom(src => src.Test)
            )
        .ForCtorParam("MyInt",
        opt =>
            opt.MapFrom(src => src.Nested.AAAA)
            )
        );
//configuration.AssertConfigurationIsValid();
var mapper2 = configuration2.CreateMapper();

var in1 = new In1
{
    Test = "PPPPPP",
    Nested = new In2 { AAAA = 16 }
};

var result2 = mapper2.Map<In1, Out>(in1);
#endregion

Console.WriteLine();