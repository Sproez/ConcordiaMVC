using ConcordiaLib.Domain;

namespace ConcordiaLib.Collections;

public class DatabaseImage
{
    public List<CardList> CardLists { get; init; }
    public List<Card> Cards { get; init; }
    public List<Person> People { get; init; }
    public List<Comment> Comments { get; init; }
    public List<Assignment> Assignments { get; init; }

    public DatabaseImage()
    {
        Cards = new List<Card>();
        People = new List<Person>();
        Comments = new List<Comment>();
        Assignments = new List<Assignment>();
        CardLists = new List<CardList>();
    }

    public DatabaseImage(
        List<Card> cards,
        List<Person> people,
        List<Comment> comments,
        List<Assignment> assignments,
        List<CardList> cardLists
        )
    {
        Cards = cards;
        People = people;
        Comments = comments;
        Assignments = assignments;
        CardLists = cardLists;
    }
}



