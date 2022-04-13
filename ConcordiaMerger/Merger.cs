using ConcordiaLib.Domain;

namespace ConcordiaMerger;

public class Merger
{
    private DatabaseImage localData;
    private DatabaseImage remoteData;

    public Merger(DatabaseImage local, DatabaseImage remote)
    {
        localData = local;
        remoteData = remote;
    }

    public (DatabaseImage created, DatabaseImage updated, DatabaseImage deleted) Merge()
    {
        var cardListMerge = MergeCardLists();
        var peopleMerge = MergePeople();
        var cardsMerge = MergeCards();
        var commentsMerge = MergeComments();
        var assignmentsMerge = MergeAssignments();

        return (
            new DatabaseImage(cardsMerge.Item1, peopleMerge.Item1, commentsMerge.Item1, assignmentsMerge.Item1, cardListMerge.Item1),
            new DatabaseImage(cardsMerge.Item2, peopleMerge.Item2, commentsMerge.Item2, assignmentsMerge.Item2, cardListMerge.Item2),
            new DatabaseImage(cardsMerge.Item3, peopleMerge.Item3, commentsMerge.Item3, assignmentsMerge.Item3, cardListMerge.Item3)
            );
    }

    private (List<CardList>, List<CardList>, List<CardList>) MergeCardLists()
    {
        var created = new List<CardList>();
        var updated = new List<CardList>();
        var deleted = new List<CardList>();

        var localDict = localData.CardLists.ToDictionary(i => i.Id);
        var remoteDict = remoteData.CardLists.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value) updated.Add(kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                deleted.Add(kvp.Value);
            }
        }

        return (created, updated, deleted);
    }

    private (List<Person>, List<Person>, List<Person>) MergePeople()
    {
        var updated = new List<Person>();
        var created = new List<Person>();
        var deleted = new List<Person>();

        var localDict = localData.People.ToDictionary(i => i.Id);
        var remoteDict = remoteData.People.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value) updated.Add(kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                deleted.Add(kvp.Value);
            }
        }

        return (created, updated, deleted);
    }

    private (List<Card>, List<Card>, List<Card>) MergeCards()
    {
        var updated = new List<Card>();
        var created = new List<Card>();
        var deleted = new List<Card>();

        var localDict = localData.Cards.ToDictionary(i => i.Id);
        var remoteDict = remoteData.Cards.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value)
                {
                    //TODO
                    //proper updates
                    updated.Add(kvp.Value);
                }
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                deleted.Add(kvp.Value);
            }
        }

        return (created, updated, deleted);
    }

    private (List<Comment>, List<Comment>, List<Comment>) MergeComments()
    {
        var updated = new List<Comment>();
        var created = new List<Comment>();
        var deleted = new List<Comment>();

        var localDict = localData.Comments.ToDictionary(i => i.CardId);
        var remoteDict = remoteData.Comments.ToDictionary(i => i.CardId);

        foreach (var kvp in remoteDict)
        {
            //First comment on card, made remotely
            if (!localDict.ContainsKey(kvp.Key))
            {
                created.Add(kvp.Value);
            }
            else
            {
                if (localDict[kvp.Key].Id == kvp.Value.Id)
                {
                    //Edited comment
                    if (localDict[kvp.Key] != kvp.Value) updated.Add(kvp.Value);
                }
                else
                {
                    //Keep newest comment, delete older one
                    if (localDict[kvp.Key].CreatedAt > kvp.Value.CreatedAt)
                    {
                        created.Add(localDict[kvp.Key]);
                        deleted.Add(kvp.Value);
                    }
                    else
                    {
                        created.Add(kvp.Value);
                        deleted.Add(localDict[kvp.Key]);
                    }
                }
            }
        }

        foreach (var kvp in localDict)
        {
            //First comment on card, made locally
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                //TODO
                //Post comment on Trello
                //created.Add(kvp.Value);
            }
        }

        return (created, updated, deleted);
    }

    private (List<Assignment>, List<Assignment>, List<Assignment>) MergeAssignments()
    {
        var created = new List<Assignment>();
        var deleted = new List<Assignment>();

        var localDict = localData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        var remoteDict = remoteData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                created.Add(kvp.Value);
            }
            //Else assignment already exists, and there is no need to update it
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                deleted.Add(kvp.Value);
            }
        }

        //Assignments are never updated
        var updated = new List<Assignment>();
        return (created, updated, deleted);
    }

}
