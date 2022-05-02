namespace ConcordiaLib.Collections;

using Domain;

public class MergingResults
{
    public MergeLocalRemote<Card> Cards { get; init; }
    public MergeLocalRemote<Assignment> Assignments { get; init; }
    public MergeLocalRemote<Person> People { get; init; }
    public MergeLocalRemote<Comment> Comments { get; init; }
    public MergeLocalRemote<CardList> CardLists { get; init; }

    public MergingResults(
        MergeLocalRemote<Card>? cards = null,
        MergeLocalRemote<Assignment>? assignments = null,
        MergeLocalRemote<Person>? people = null,
        MergeLocalRemote<Comment>? comments = null,
        MergeLocalRemote<CardList>? cardLists = null
        )
    {
        Cards = cards ?? new MergeLocalRemote<Card>();
        Assignments = assignments ?? new MergeLocalRemote<Assignment>();
        People = people ?? new MergeLocalRemote<Person>();
        Comments = comments ?? new MergeLocalRemote<Comment>();
        CardLists = cardLists ?? new MergeLocalRemote<CardList>();
    }
}

