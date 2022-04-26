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
        List<CardList> expectedRemoteUpdated = null!;
        

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
        //Local can update remote lists
        expectedRemoteUpdated = new List<CardList>() { cardListL1 };
    }

    [Test]
    public void Test()
    {
        //Merge should not create or delete remote
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Deleted);
        //Merge should not update or delete local
        Assert.IsEmpty(result.Local.Updated);
        Assert.IsEmpty(result.Local.Deleted);
        //Merge can update on remote
        Assert.AreEqual(expectedRemoteUpdated, result.Remote.Updated);
        //Merge can create on local
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);

    }
}


