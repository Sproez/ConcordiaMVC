using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Collections;
using ConcordiaLib.Domain;
using ConcordiaLib.Enum;
using ConcordiaMerger;
using NUnit.Framework;

namespace UnitTests.MergerFolder;

public class CommentTest
{
    MergeLocalRemote<Comment> result = null!;

    List<Comment> expectedLocalCreated = null!;
    List<Comment> expectedLocalUpdated = null!;
    List<Comment> expectedLocalDeleted = null!;
    List<Comment> expectedRemoteCreated = null!;
    List<Comment> expectedRemoteDeleted = null!;

    [SetUp]
    public void Setup()
    {
        //Comments will be deleted if they don't belong to an existing card
        var card1 = new Card("A", "a", null, Priority.Default) { Id = "1", CardListId = "1" };
        var card2 = new Card("A", "a", null, Priority.Default) { Id = "2", CardListId = "1" };
        var card3 = new Card("A", "a", null, Priority.Default) { Id = "3", CardListId = "1" };
        var card4 = new Card("A", "a", null, Priority.Default) { Id = "4", CardListId = "1" };
        var card5 = new Card("A", "a", null, Priority.Default) { Id = "5", CardListId = "1" };

        var cardList = new List<Card> { card1, card2, card3, card4, card5 };

        //Pair, different ids and local is newer (local should be created on remote, remote should be deleted on remote)
        var commentL1 = new Comment("LocalName", new DateTime(2000, 1, 2))
        { Id = "1", CardId = "1", PersonId = "1" };
        var commentR1 = new Comment("RemoteName", new DateTime(2000, 1, 1))
        { Id = "2", CardId = "1", PersonId = "2" };
        //Local-only (should be mirrored on remote)
        var commentL2 = new Comment("LocalOnly", new DateTime(2000, 1, 1))
        { Id = "3", CardId = "2", PersonId = "1" };
        //Remote-only (should be mirrored on local)
        var commentR2 = new Comment("RemoteOnly", new DateTime(2000, 1, 1))
        { Id = "4", CardId = "3", PersonId = "1" };
        //Pair, different ids and remote is newer (remote should be created on local, local should be deleted on both local and remote)
        var commentL3 = new Comment("LocalName2", new DateTime(2000, 1, 1))
        { Id = "5", CardId = "4", PersonId = "1" };
        var commentR3 = new Comment("RemoteName2", new DateTime(2000, 1, 2))
        { Id = "6", CardId = "4", PersonId = "2" };
        //Pair, same id (local should be updated to match remote)
        var commentL4 = new Comment("OriginalName", new DateTime(2000, 1, 2))
        { Id = "7", CardId = "5", PersonId = "1" };
        var commentR4 = new Comment("ModifiedName", new DateTime(2000, 1, 1))
        { Id = "7", CardId = "5", PersonId = "2" };
        //Remote-only on a non-existing card (should be deleted on remote) TODO
        var commentR5 = new Comment("RemoteCommentOnDeletedCard", new DateTime(2000, 1, 1))
        { Id = "8", CardId = "6", PersonId = "1" };


        var localList = new List<Comment> { commentL1, commentL2, commentL3, commentL4 };
        var remoteList = new List<Comment> { commentR1, commentR2, commentR3, commentR4, commentR5 };

        var localImage = new DatabaseImage() { Comments = localList, Cards = cardList };
        var remoteImage = new DatabaseImage() { Comments = remoteList, Cards = cardList };

        result = new Merger(localImage, remoteImage).Merge().Comments;

        //Local can create remote comments
        expectedRemoteCreated = new List<Comment>() { commentL1, commentL2 };
        //Local and remote can delete remote comments
        expectedRemoteDeleted = new List<Comment>() { commentR1, commentL3, commentR5 };
        //Remote can create local comments
        expectedLocalCreated = new List<Comment>() { commentR2, commentR3 };
        //Remote can update local comments
        expectedLocalUpdated = new List<Comment>() { commentR4 };
        //Remote can delete local comments
        expectedLocalDeleted = new List<Comment>() { commentL3 };

    }

    [Test]
    public void Test()
    {
        //Merge should not update on remote
        Assert.IsEmpty(result.Remote.Updated);
        //Merge can create and delete on remote
        Assert.AreEqual(expectedRemoteCreated, result.Remote.Created);
        Assert.AreEqual(expectedRemoteDeleted, result.Remote.Deleted);
        //Merge can create, update, and delete on local
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        Assert.AreEqual(expectedLocalUpdated, result.Local.Updated);
        Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);
    }

}

