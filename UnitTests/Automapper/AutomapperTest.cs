using System;
using System.Collections.Generic;
using AutoMapper;
using ConcordiaLib.Domain;
using ConcordiaLib.Enum;
using ConcordiaTrelloClient.Dto;
using ConcordiaTrelloClient.Dto.NestedProperties;
using ConcordiaTrelloClient.Mapping;
using ConcordiaTrelloClient.Options;
using NUnit.Framework;

namespace UnitTests.Automapper;

public class AutomapperTest
{

    IMapper mapper = null!;

    [SetUp]
    public void Setup()
    {
        var options = new ApiOptions();
        var pResolver = new PriorityResolver(options);
        var automapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CardDto, Card>()
                .ForCtorParam("Priority",
                opt => opt.MapFrom(src => pResolver.Resolve(src.LabelIds)));
            cfg.CreateMap<CardListDto, CardList>();
            cfg.CreateMap<PersonDto, Person>();
            cfg.CreateMap<CommentDto, Comment>();
        });

        mapper = automapperConfig.CreateMapper();
    }

    [Test]
    public void PersonTest()
    {
        var entity = new Person("A") { Id = "B" };
        var dto = new PersonDto() { Name = "A", Id = "B" };

        var mappedEntity = mapper.Map<PersonDto, Person>(dto);

        Assert.AreEqual(entity, mappedEntity);
    }

    [Test]
    public void CardListTest()
    {
        var entity = new CardList("A") { Id = "B" };
        var dto = new CardListDto() { Name = "A", Id = "B" };

        var mappedEntity = mapper.Map<CardListDto, CardList>(dto);

        Assert.AreEqual(entity, mappedEntity);
    }

    [Test]
    public void CommentTest()
    {
        var entity = new Comment("A", new DateTime(2000, 1, 1)) { Id = "B", CardId = "C", PersonId = "D" };
        var dto = new CommentDto()
        {
            Id = "B",
            CreatedAt = new DateTime(2000, 1, 1),
            PersonId = "D",
            Data = new NestedData()
            {
                Text = "A",
                Card = new NestedCard()
                {
                    Id = "C",
                    AssigneesIds = new List<string>() { "E", "F", "G" }
                }
            }
        };

        var mappedEntity = mapper.Map<CommentDto, Comment>(dto);

        Assert.AreEqual(entity, mappedEntity);
    }

    [Test]
    public void CardTest()
    {
        var entity = new Card("A", "B", new DateTime(2000, 1, 1), Priority.High) { Id = "C", CardListId = "D" };
        var dto = new CardDto()
        {
            Title = "A",
            Description = "B",
            DueBy = new DateTime(2000, 1, 1),
            LabelIds = new List<string>() { "625032fb182ca5704dde89f7" },
            Id = "C",
            CardListId = "D"
        };

        var mappedEntity = mapper.Map<CardDto, Card>(dto);

        Assert.AreEqual(entity, mappedEntity);
    }

    //Doesn't use Automapper but whatever
    [Test]
    public void AssignmentTest()
    {
        var entityA = new Assignment() { PersonId = "A", CardId = "Z" };
        var entityB = new Assignment() { PersonId = "B", CardId = "Z" };
        var entityC = new Assignment() { PersonId = "C", CardId = "Z" };
        var dto = new NestedCard()
        {
            Id = "Z",
            AssigneesIds = new List<string>() { "A", "B", "C" }
        };

        var expected = new HashSet<Assignment>() { entityA, entityB, entityC };
        var result = new HashSet<Assignment>();

        foreach (var aId in dto.AssigneesIds)
        {
            var assignment = new Assignment()
            {
                CardId = dto.Id,
                PersonId = aId
            };
            result.Add(assignment);
        }

        Assert.AreEqual(expected, result);
    }
}

