using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcordiaLib.Domain;

public class DatabaseImage
{
    public List<CardList> CardLists { get; init; }
    public List<Card> Cards { get; init; }
    public List<Person> People { get; init; }
    public List<Comment> Comments { get; init; }
    public List<Assignment> Assignments { get; init; }

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



