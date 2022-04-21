namespace ConcordiaMerger;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using MergeLogic;
using Helpers;

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
        MergeLocalRemote<CardList> cardListMerge = MergeHelper.Merge<CardList, string>(
            cardListLocalDict, cardListRemoteDict,
            CardListLogic.GetMergeActions()
            );

        //People
        var peopleLocalDict = localData.People.ToDictionary(i => i.Id);
        var peopleRemoteDict = remoteData.People.ToDictionary(i => i.Id);
        MergeLocalRemote<Person> peopleMerge = MergeHelper.Merge<Person, string>(
            peopleLocalDict, peopleRemoteDict,
            PersonLogic.GetMergeActions()
            );

        //Cards
        var cardsLocalDict = localData.Cards.ToDictionary(i => i.Id);
        var cardsRemoteDict = remoteData.Cards.ToDictionary(i => i.Id);
        MergeLocalRemote<Card> cardsMerge = MergeHelper.Merge<Card, string>(
            cardsLocalDict, cardsRemoteDict,
            CardLogic.GetMergeActions()
            );

        //Comments

        //Keep newest comments on cards, keep older comments to delete them later
        var (latestComments, outdatedComments) = CommentsHelper.SeparateNewAndOldComments(remoteData.Comments);
        //Keep comments on existing cards, keep the rest to delete them later
        var (validComments, badComments) = CommentsHelper.SeparateValidAndBadComments(latestComments, localData.Cards, cardsMerge);

        var commentsLocalDict = localData.Comments.ToDictionary(i => i.CardId);
        var commentsRemoteDict = validComments.ToDictionary(i => i.CardId);
        MergeLocalRemote<Comment> commentsMerge = MergeHelper.Merge<Comment, string>(
            commentsLocalDict, commentsRemoteDict,
            CommentLogic.GetMergeActions()
            );

        commentsMerge.Remote.Deleted.AddRange(outdatedComments);
        commentsMerge.Remote.Deleted.AddRange(badComments);

        //Assignemnt
        var assignmentsLocalDict = localData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        var assignmentsRemoteDict = remoteData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        MergeLocalRemote<Assignment> assignmentsMerge = MergeHelper.Merge<Assignment, (string, string)>(
            assignmentsLocalDict, assignmentsRemoteDict,
            AssignmentLogic.GetMergeActions()
            );

        //
        MergingResults result = new MergingResults(cardsMerge, assignmentsMerge, peopleMerge, commentsMerge, cardListMerge);
        return result;
    }

}