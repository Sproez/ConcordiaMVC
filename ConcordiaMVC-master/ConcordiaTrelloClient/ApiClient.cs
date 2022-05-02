using ConcordiaTrelloClient.ApiInputOutput;
using ConcordiaTrelloClient.Mapping;

namespace ConcordiaTrelloClient;

using AutoMapper;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Dto;
using ConcordiaLib.Domain;
using ConcordiaLib.Abstract;
using ConcordiaLib.Collections;
using Options;

public class ApiClient : IApiClient
{
    public readonly ApiOptions options;
    public readonly IMapper mapper;
    public readonly HttpClient httpClient;

    public readonly string BoardEndpoint;

    private readonly ApiReader _reader;
    private readonly ApiWriter _writer;

    public ApiClient(ApiOptions options)
    {
        //Options config
        this.options = options;
        BoardEndpoint = $"{this.options.BaseURL}/boards/{this.options.ConcordiaBoardID}";

        //Automapper setup
        var pResolver = new PriorityResolver(this.options);

        var automapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CardDto, Card>()
                .ForCtorParam("Priority",
                opt => opt.MapFrom(src => pResolver.Resolve(src.LabelIds)));
            cfg.CreateMap<CardListDto, CardList>();
            cfg.CreateMap<PersonDto, Person>();
            cfg.CreateMap<CommentDto, Comment>();
        });

        var mapper = automapperConfig.CreateMapper();
        this.mapper = mapper;

        //HTTP client setup and config
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Concordia Trello Client");

        //Reader and writer setup
        _reader = new ApiReader(this);
        _writer = new ApiWriter(this);
    }

    public async Task<DatabaseImage> GetDataFromApiAsync()
    {
        return await _reader.GetDataFromApiAsync();
    }

    public async Task PutDataToApiAsync(MergingResults merge)
    {
        await _writer.PutDataToApiAsync(merge);
    }

}


