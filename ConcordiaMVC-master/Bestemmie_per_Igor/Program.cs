using AutoMapper;
using Bestemmie_per_Igor;

var configuration = new MapperConfiguration(
    cfg =>
        cfg.CreateMap<Source, Destination>()
        .ForCtorParam("MoreMemes",    //Breaks MapFrom?
        //.ForMember(src => src.MoreMemes,
        opt => 
            opt.MapFrom(dst => PINGAS.cancer(dst.Memes))
            )
        );


//configuration.AssertConfigurationIsValid();
var mapper = configuration.CreateMapper();

var source = new Source
{
    Title = "A",
    Id = "C",
    Memes = "123456"
};

var result = mapper.Map<Source, Destination>(source);

Console.WriteLine();