namespace ConcordiaLib.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Enum;

public class CardComparer : Comparer<Card>
{
    private readonly TimeSpan _soon = new TimeSpan(5, 0, 0, 0);
    private readonly string _completedListId;

    public CardComparer(string cId)
    {
        _completedListId = cId;
    }
    // Compares by Length, Height, and Width.
    public override int Compare(Card? x, Card? y)
    {
        //Boring sort properties
        if (x is null && y is null) return 0;
        if (x is not null && y is null) return -1;
        if (x is null && y is not null) return 1;
        //Here we know both x and y are not null, but the compiler doesn't

        //Score values:
        //High priority: 4
        //Due by in <5 days : 3
        //Medium priority: 2
        //Low priority: 1
        //Default priority: 0

        int HighP(Card c) => c.Priority == Priority.High ? 1 : 0;
        bool DueEarly(Card c) => c.DueBy is not null && (c.DueBy - DateTime.Now) < _soon && c.Priority != Priority.High;

        int xScore = (int)x!.Priority + HighP(x);
        xScore = DueEarly(x) ? 3 : xScore;
        int yScore = (int)y!.Priority + HighP(y);
        yScore = DueEarly(y) ? 3 : yScore;

        return yScore - xScore;
    }
}
