using System.Collections.Generic;
using NUnit.Framework;
using ConcordiaMerger;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;

namespace UnitTests.MergerFolder;

public class PersonTest
{
    MergeLocalRemote<Person> result = null!;

    List<Person> expectedLocalCreated = null!;
    List<Person> expectedLocalUpdated = null!;
    List<Person> expectedLocalDeleted = null!;

    [SetUp]
    public void Setup()
    {
        //Pair (local should be updated to match remote)
        var personL1 = new Person("LocalName") { Id = "1" };
        var personR1 = new Person("RemoteName") { Id = "1" };
        //Local-only (should be deleted)
        var personL2 = new Person("LocalName2") { Id = "2" };
        //Remote-only (should be mirrored on local)
        var personR2 = new Person("RemoteName2") { Id = "3" };

        var localList = new List<Person> { personL1, personL2 };
        var remoteList = new List<Person> { personR1, personR2 };

        var localImage = new DatabaseImage() { People = localList };
        var remoteImage = new DatabaseImage() { People = remoteList };

        result = new Merger(localImage, remoteImage).Merge().People;

        //Remote can create, update, and delete local people
        expectedLocalCreated = new List<Person>() { personR2 };
        expectedLocalUpdated = new List<Person>() { personR1 };
        expectedLocalDeleted = new List<Person>() { personL2 };
    }

    [Test]
    public void Test()
    {
        //Merge should not affect remote
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Updated);
        Assert.IsEmpty(result.Remote.Deleted);

        //Merge can create, update, and delete on local
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        Assert.AreEqual(expectedLocalUpdated, result.Local.Updated);
        Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);
    }


}

