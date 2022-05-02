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

public class AssignmentTest
{
    MergeLocalRemote<Assignment> result = null!;

    List<Assignment> expectedLocalCreated = null!;
    List<Assignment> expectedLocalDeleted = null!;


    [SetUp]
    public void Setup()
    {
        //Pair (no action needed)
        var assignmentL1 = new Assignment() { CardId = "1", PersonId = "1" };
        var assignmentR1 = new Assignment() { CardId = "1", PersonId = "1" };
        //Local-only (should be deleted)
        var assignmentL2 = new Assignment() { CardId = "2", PersonId = "3" };
        //Remote-only (should be mirrored on local)
        var assignmentR2 = new Assignment() { CardId = "3", PersonId = "2" };

        var localList = new List<Assignment> { assignmentL1, assignmentL2 };
        var remoteList = new List<Assignment> { assignmentR1, assignmentR2 };

        var localImage = new DatabaseImage() { Assignments = localList };
        var remoteImage = new DatabaseImage() { Assignments = remoteList };

        result = new Merger(localImage, remoteImage).Merge().Assignments;

        //Remote can create and delete local assignments
        expectedLocalCreated = new List<Assignment>() { assignmentR2 };
        expectedLocalDeleted = new List<Assignment>() { assignmentL2 };
    }

    [Test]
    public void Test()
    {
        //Merge should not update local
        Assert.IsEmpty(result.Local.Updated);
        //Merge should not affect remote
        Assert.IsEmpty(result.Remote.Created);
        Assert.IsEmpty(result.Remote.Updated);
        Assert.IsEmpty(result.Remote.Deleted);
        //Merge can create and delete on local
        Assert.AreEqual(expectedLocalCreated, result.Local.Created);
        Assert.AreEqual(expectedLocalDeleted, result.Local.Deleted);
    }
}

