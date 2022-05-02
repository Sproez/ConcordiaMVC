using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Collections;
using ConcordiaLib.Domain;

namespace ConcordiaMerger.Helpers;

public static class CommentsHelper
{
    public static (List<Comment>, List<Comment>) SeparateNewAndOldComments(List<Comment> input)
    {
        var resultKeep = new List<Comment>();
        ILookup<string, Comment> temp = input.ToLookup(c => c.CardId, c => c);
        foreach (var commentsInCard in temp)
        {
            Comment newestComment = commentsInCard.OrderByDescending(c => c.CreatedAt).First();
            resultKeep.Add(newestComment);
        }
        var resultDiscard = input.Where(c => !resultKeep.Contains(c)).ToList();
        return (resultKeep, resultDiscard);
    }

    //The API also returns comments on deleted cards, this method separates those from the rest
    public static (List<Comment>, List<Comment>) SeparateValidAndBadComments(List<Comment> input, List<Card> localCards, MergeLocalRemote<Card> cardMerge)
    {
        var validCards = new List<Card>();
        //A card is valid if it exists and is not scheduled for deletion, or if it is scheduled for creation
        validCards.AddRange(localCards);
        validCards.RemoveAll(card => cardMerge.Local.Deleted.Contains(card));
        validCards.AddRange(cardMerge.Local.Created);

        var validCardsDict = validCards.ToDictionary(card => card.Id);

        var resultKeep = input.Where(com => validCardsDict.ContainsKey(com.CardId)).ToList();
        var resultDiscard = input.Where(com => !validCardsDict.ContainsKey(com.CardId)).ToList();

        return (resultKeep, resultDiscard);
    }
}

