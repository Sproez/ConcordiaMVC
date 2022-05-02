using System.Collections.Generic;
using NUnit.Framework;
using ConcordiaMerger;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using ConcordiaLib.Enum;
using System;

namespace UnitTests.MergerFolder
{
    public class CardsTest
    {
        MergeLocalRemote<Card> result = null!;

        List<Card> expectedLocalCreated = null!;
        List<Card> expectedLocalUpdated = null!;
        List<Card> expectedLocalDeleted = null!;
        List<Card> expectedRemoteUpdated = null!;

        [SetUp]
        public void Setup()
        {
            //Pair (should be updated to the merged version, both on local and remote)
            var cardL1 = new Card("tL", "localDesc", null, Priority.Default)
            { Id = "1", CardListId = "2" };
            var cardR1 = new Card("tR", "remoteDesc", new DateTime(2000, 1, 1), Priority.High)
            { Id = "1", CardListId = "1" };

            var cardM = new Card(cardR1.Title, cardR1.Description, cardR1.DueBy, cardR1.Priority)
            { Id = cardL1.Id, CardListId = cardL1.CardListId };
            //Local-only (should be deleted)
            var cardL2 = new Card("tL", "localOnly", new DateTime(2000, 1, 2), Priority.Medium)
            { Id = "2", CardListId = "1" };
            //Remote-only (should be mirrored on local)
            var cardR2 = new Card("tR", "remoteOnly", new DateTime(2000, 1, 3), Priority.Low)
            { Id = "3", CardListId = "1" };

            var localList = new List<Card> { cardL1, cardL2 };
            var remoteList = new List<Card> { cardR1, cardR2 };

            var localImage = new DatabaseImage() { Cards = localList };
            var remoteImage = new DatabaseImage() { Cards = remoteList };

            result = new Merger(localImage, remoteImage).Merge().Cards;


            //Remote can create local cards
            expectedLocalCreated = new List<Card>() { cardR2 };
            //Remote can delete local cards
            expectedLocalDeleted = new List<Card>() { cardL2 };
            //Both remote and local can update cards
            expectedLocalUpdated = new List<Card>() { cardM };
            expectedRemoteUpdated = new List<Card>() { cardM };

        }

        [Test]
        public void Test()
        {
            //Merge should not create or delete on remote
            Assert.IsEmpty(result.Remote.Created);
            Assert.IsEmpty(result.Remote.Deleted);
            //Merge can update on remote
            Assert.AreEqual(expectedRemoteUpdated, result.Remote.Updated);
            //Merge can create, update, and delete on local
            Assert.AreEqual(expectedLocalCreated, result.Local.Created);
            Assert.AreEqual(expectedLocalUpdated, result.Local.Updated);
            Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);

        }

    }
}
