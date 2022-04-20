using System.Collections.Generic;
using NUnit.Framework;
using ConcordiaMerger;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using ConcordiaLib.Enum;
using System;

namespace UnitTests;

public class MergerTests
{
    [SetUp]
    public void Setup()
    {


    }

    [Test]
    public void MergeTest()
    {
        Assert.Pass();
    }

    [Test]
    public void MergeCardListTest()
    {
        var cardListL1 = new CardList("LocalName") { Id = "1" };
        var cardListR1 = new CardList("RemoteName") { Id = "1" };
        var cardListL2 = new CardList("LocalOnly") { Id = "2" };
        var cardListR2 = new CardList("RemoteOnly") { Id = "3" };

        var localList = new List<CardList> { cardListL1, cardListL2 };
        var remoteList = new List<CardList> { cardListR1, cardListR2 };

        var localImage = new DatabaseImage() { CardLists = localList };
        var remoteImage = new DatabaseImage() { CardLists = remoteList };

        var result = new Merger(localImage, remoteImage).Merge().CardLists;

        var expectedLocalCreated = new List<CardList>() { cardListR2 };
        var expectedRemoteCreated = new List<CardList>() { cardListL1 };

        //Remote can't update or delete local lists
        Assert.IsEmpty(result.Local.Updated);
        Assert.IsEmpty(result.Local.Deleted);
        //Local can't create or delete lists remotely
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Remote can create local lists
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Local can update remote lists
        Assert.AreEqual(expectedRemoteCreated, result.Remote.Updated);
    }

    [Test]
    public void MergeCardsTest()
    {
        var cardL1 = new Card("tL", "localDesc", null, Priority.Default) { Id = "1", CardListId = "2" };
        var cardR1 = new Card("tR", "remoteDesc", new DateTime(2000, 1, 1), Priority.High) { Id = "1", CardListId = "1" };
        var cardL2 = new Card("tL", "localDesc", new DateTime(2000, 1, 2), Priority.Medium) { Id = "2", CardListId = "1" };
        var cardR2 = new Card("tR", "remoteDesc", new DateTime(2000, 1, 3), Priority.Low) { Id = "3", CardListId = "1" };

        var localList = new List<Card> { cardL1, cardL2 };
        var remoteList = new List<Card> { cardR1, cardR2 };

        var localImage = new DatabaseImage() { Cards = localList };
        var remoteImage = new DatabaseImage() { Cards = remoteList };

        var result = new Merger(localImage, remoteImage).Merge().Cards;

        var cardM = new Card("tR", "remoteDesc", new DateTime(2000, 1, 1), Priority.High) { Id = "1", CardListId = "2" };

        var expectedLocalCreated = new List<Card>() { cardR2 };
        var expectedLocalDeleted = new List<Card>() { cardL2 };
        var expectedLocalUpdated = new List<Card>() { cardM };
        var expectedRemoteUpdated = new List<Card>() { cardM };

        //Local can't create or delete lists remotely
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Remote can create local cards
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Remote can delete local cards
        Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);
        //Both remote and local can update cards
        Assert.AreEqual(expectedLocalUpdated, result.Local.Updated);
        Assert.AreEqual(expectedRemoteUpdated, result.Remote.Updated);

    }

    public void MergePeopleTest()
    {
        var cardListL1 = new CardList("LocalName") { Id = "1" };
        var cardListR1 = new CardList("RemoteName") { Id = "1" };
        var cardListL2 = new CardList("LocalOnly") { Id = "2" };
        var cardListR2 = new CardList("RemoteOnly") { Id = "3" };

        var localList = new List<CardList> { cardListL1, cardListL2 };
        var remoteList = new List<CardList> { cardListR1, cardListR2 };

        var localImage = new DatabaseImage() { CardLists = localList };
        var remoteImage = new DatabaseImage() { CardLists = remoteList };

        var result = new Merger(localImage, remoteImage).Merge().CardLists;

        var expectedLocalCreated = new List<CardList>() { cardListR2 };
        var expectedResultRemote = new List<CardList>() { cardListL1 };

        //Remote can't update or delete local lists
        Assert.IsEmpty(result.Local.Updated);
        Assert.IsEmpty(result.Local.Deleted);
        //Local can't create or delete lists remotely
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Remote can create local lists
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Local can update remote lists
        Assert.AreEqual(expectedResultRemote, result.Remote.Updated);
    }

    public void MergeAssignmentsTest()
    {
        var cardListL1 = new CardList("LocalName") { Id = "1" };
        var cardListR1 = new CardList("RemoteName") { Id = "1" };
        var cardListL2 = new CardList("LocalOnly") { Id = "2" };
        var cardListR2 = new CardList("RemoteOnly") { Id = "3" };

        var localList = new List<CardList> { cardListL1, cardListL2 };
        var remoteList = new List<CardList> { cardListR1, cardListR2 };

        var localImage = new DatabaseImage() { CardLists = localList };
        var remoteImage = new DatabaseImage() { CardLists = remoteList };

        var result = new Merger(localImage, remoteImage).Merge().CardLists;

        var expectedLocalCreated = new List<CardList>() { cardListR2 };
        var expectedResultRemote = new List<CardList>() { cardListL1 };

        //Remote can't update or delete local lists
        Assert.IsEmpty(result.Local.Updated);
        Assert.IsEmpty(result.Local.Deleted);
        //Local can't create or delete lists remotely
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Remote can create local lists
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Local can update remote lists
        Assert.AreEqual(expectedResultRemote, result.Remote.Updated);
    }

    public void MergeCommentsTest()
    {
        var cardListL1 = new CardList("LocalName") { Id = "1" };
        var cardListR1 = new CardList("RemoteName") { Id = "1" };
        var cardListL2 = new CardList("LocalOnly") { Id = "2" };
        var cardListR2 = new CardList("RemoteOnly") { Id = "3" };

        var localList = new List<CardList> { cardListL1, cardListL2 };
        var remoteList = new List<CardList> { cardListR1, cardListR2 };

        var localImage = new DatabaseImage() { CardLists = localList };
        var remoteImage = new DatabaseImage() { CardLists = remoteList };

        var result = new Merger(localImage, remoteImage).Merge().CardLists;

        var expectedLocalCreated = new List<CardList>() { cardListR2 };
        var expectedResultRemote = new List<CardList>() { cardListL1 };

        //Remote can't update or delete local lists
        Assert.IsEmpty(result.Local.Updated);
        Assert.IsEmpty(result.Local.Deleted);
        //Local can't create or delete lists remotely
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Remote can create local lists
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Local can update remote lists
        Assert.AreEqual(expectedResultRemote, result.Remote.Updated);
    }
}
