namespace ConcordiaMerger;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using MergeLogic;

public class Merger
{
    private DatabaseImage localData;
    private DatabaseImage remoteData;

    public Merger(DatabaseImage local, DatabaseImage remote)
    {
        localData = local;
        remoteData = remote;
    }

    public MergingResults Merge()
    {
        //Card lists
        var cardListLocalDict = localData.CardLists.ToDictionary(i => i.Id);
        var cardListRemoteDict = remoteData.CardLists.ToDictionary(i => i.Id);
        var cardListMerge = Merge<CardList, string>(
            cardListLocalDict, cardListRemoteDict,
            CardListLogic.MergeWhenOnlyLocal,
            CardListLogic.MergeWhenOnlyRemote,
            CardListLogic.MergeWhenConflict
            );

        //People
        var peopleLocalDict = localData.People.ToDictionary(i => i.Id);
        var peopleRemoteDict = remoteData.People.ToDictionary(i => i.Id);
        var peopleMerge = Merge<Person, string>(
            peopleLocalDict, peopleRemoteDict,
            PersonLogic.MergeWhenOnlyLocal,
            PersonLogic.MergeWhenOnlyRemote,
            PersonLogic.MergeWhenConflict
            );

        //Cards
        var cardsLocalDict = localData.Cards.ToDictionary(i => i.Id);
        var cardsRemoteDict = remoteData.Cards.ToDictionary(i => i.Id);
        var cardsMerge = Merge<Card, string>(
            cardsLocalDict, cardsRemoteDict,
            CardLogic.MergeWhenOnlyLocal,
            CardLogic.MergeWhenOnlyRemote,
            CardLogic.MergeWhenConflict
            );

        //Comments

        //Disregard older comments on cards
        var latestComments = GetLatestComments(remoteData.Comments);
        //Disregard comments on closed cards
        var activeComments = GetOnlyCommentsOnOpenCards(latestComments);

        var commentsLocalDict = localData.Comments.ToDictionary(i => i.CardId);
        var commentsRemoteDict = activeComments.ToDictionary(i => i.CardId);
        var commentsMerge = Merge<Comment, string>(
            commentsLocalDict, commentsRemoteDict,
            CommentLogic.MergeWhenOnlyLocal,
            CommentLogic.MergeWhenOnlyRemote,
            CommentLogic.MergeWhenConflict
            );

        //Assignemnt
        var assignmentsLocalDict = localData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        var assignmentsRemoteDict = remoteData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        var assignmentsMerge = Merge<Assignment, (string, string)>(
            assignmentsLocalDict, assignmentsRemoteDict,
            AssignmentLogic.MergeWhenOnlyLocal,
            AssignmentLogic.MergeWhenOnlyRemote,
            AssignmentLogic.MergeWhenConflict
            );

        var result = new MergingResults(cardsMerge, assignmentsMerge, peopleMerge, commentsMerge, cardListMerge);
        return result;
    }

    private MergeLocalRemote<T> Merge<T, Tkey>(
        Dictionary<Tkey, T> localDict, Dictionary<Tkey, T> remoteDict,
        Action<MergeLocalRemote<T>, T> MergeWhenOnlyLocal,
        Action<MergeLocalRemote<T>, T> MergeWhenOnlyRemote,
        Action<MergeLocalRemote<T>, T, T> MergeWhenConflict)
    {
        var m = new MergeLocalRemote<T>();

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                MergeWhenOnlyRemote(m, kvp.Value);
            }
            else
            {
                //Update
                MergeWhenConflict(m, localDict[(Tkey)kvp.Key], kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletion
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                MergeWhenOnlyLocal(m, kvp.Value);
            }
        }

        return m;
    }

    private List<Comment> GetLatestComments(List<Comment> input)
    {
        var result = new List<Comment>();
        ILookup<string, Comment> temp = input.ToLookup(c => c.CardId, c => c);
        foreach (var commentsInCard in temp)
        {
            Comment newestComment = commentsInCard.OrderByDescending(c => c.CreatedAt).First();
            result.Add(newestComment);
        }
        return result;
    }

    private List<Comment> GetOnlyCommentsOnOpenCards(List<Comment> input)
    {
        var cardsLocalDict = localData.Cards.ToDictionary(i => i.Id);
        var cardsRemoteDict = remoteData.Cards.ToDictionary(i => i.Id);
        return input.Where(c => cardsLocalDict.ContainsKey(c.CardId) || cardsRemoteDict.ContainsKey(c.CardId)).ToList();
    }
}