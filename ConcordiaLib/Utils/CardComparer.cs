namespace ConcordiaLib.Utils;

using System;
using System.Collections.Generic;
using Domain;
using Enum;

public class CardComparer : Comparer<Card>
{
    //TODO use options?
    private readonly TimeSpan _soon = new TimeSpan(5, 0, 0, 0);
    private readonly string _completedListId;

    public CardComparer(string cId)
    {
        _completedListId = cId;
    }

    public override int Compare(Card? x, Card? y)
    {
        //Boring sort properties
        if (x is null && y is null) return 0;
        if (x is not null && y is null) return -1;
        if (x is null && y is not null) return 1;

        //Score values:
        //High priority: 4
        //Due by in <5 days : 3
        //Medium priority: 2
        //Low priority: 1
        //Default priority: 0
        //Completed: -1

        int PScore(Card c) =>
            c.Priority switch
            {
                Priority.High => 4,
                Priority.Medium => 2,
                Priority.Low => 1,
                _ => 0
            };

        bool DueEarly(Card c) => c.DueBy is not null && (c.DueBy - DateTime.Now) < _soon && c.Priority != Priority.High;
        bool Completed(Card c) => c.CardListId == _completedListId;

#pragma warning disable CS8604 //Here we know both x and y are not null, but the compiler doesn't
        int xScore = PScore(x);
        xScore = DueEarly(x) ? 3 : xScore;
        xScore = Completed(x) ? -1 : xScore;
        int yScore = PScore(y);
        yScore = DueEarly(y) ? 3 : yScore;
        yScore = Completed(y) ? -1 : yScore;
#pragma warning restore CS8604

        return yScore - xScore;
    }
}
