using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Collections;
using ConcordiaLib.Domain;
using ConcordiaMerger;
using NUnit.Framework;

namespace UnitTests.MergerFolder;

public class CardListTest
{
    MergeLocalRemote<CardList> result = null!;

    List<CardList> expectedLocalCreated = null!;
    List<CardList> expectedLocalUpdated = null!;
    List<CardList> expectedLocalDeleted = null!;

    [SetUp]
    public void Setup()
    {
        var cardListL1 = new CardList("LocalName") { Id = "1" };
        var cardListR1 = new CardList("RemoteName") { Id = "1" };
        var cardListL2 = new CardList("LocalOnly") { Id = "2" };
        var cardListR2 = new CardList("RemoteOnly") { Id = "3" };

        var localList = new List<CardList> { cardListL1, cardListL2 };
        var remoteList = new List<CardList> { cardListR1, cardListR2 };

        var localImage = new DatabaseImage() { CardLists = localList };
        var remoteImage = new DatabaseImage() { CardLists = remoteList };

        result = new Merger(localImage, remoteImage).Merge().CardLists;

        //Remote can create local lists
        expectedLocalCreated = new List<CardList>() { cardListR2 };
        //Remote can update local lists
        expectedLocalUpdated = new List<CardList>() { cardListR1 };
        //Remote can delete local lists
        expectedLocalDeleted = new List<CardList>() { cardListL2 };
    }

    [Test]
    public void Test()
    {
        //Merge should not create, update, or delete remote
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Updated);
        Assert.IsEmpty(result.Remote.Deleted);
        //Merge can create on local
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        //Merge can update on local
        Assert.AreEqual(expectedLocalUpdated, result.Local.Updated);
        //Merge can delete on local
        Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);

    }
}


