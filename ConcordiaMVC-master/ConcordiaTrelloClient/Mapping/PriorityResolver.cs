using AutoMapper;
using ConcordiaLib.Domain;
using ConcordiaLib.Enum;
using ConcordiaTrelloClient.Dto;
using ConcordiaTrelloClient.Options;

namespace ConcordiaTrelloClient.Mapping;

public class PriorityResolver// : IValueResolver<CardDto, Card, Priority>
{
    private readonly ApiOptions _options;

    public PriorityResolver(ApiOptions options)
    {
        _options = options;
    }

    public Priority Resolve(IEnumerable<string> labelIds)
    {
        if (labelIds.Contains(_options.HighPriorityLabelId)) return Priority.High;
        if (labelIds.Contains(_options.MediumPriorityLabelId)) return Priority.Medium;
        if (labelIds.Contains(_options.LowPriorityLabelId)) return Priority.Low;
        return Priority.Default;
    }
    /*
    public Priority Resolve(CardDto source, Card destination, Priority result, ResolutionContext context)
    {
        if (source.LabelIds.Contains(_options.HighPriorityLabelId)) return Priority.High;
        if (source.LabelIds.Contains(_options.MediumPriorityLabelId)) return Priority.Medium;
        if (source.LabelIds.Contains(_options.LowPriorityLabelId)) return Priority.Low;
        return Priority.Default;
    }
    */
}
