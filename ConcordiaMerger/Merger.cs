using ConcordiaLib.Domain;
using ConcordiaLib.Collections;

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

    public MergingResults Merge()
    {
        var cardListMerge = MergeCardLists();
        var peopleMerge = MergePeople();
        var cardsMerge = MergeCards();
        var commentsMerge = MergeComments();
        var assignmentsMerge = MergeAssignments();

        var result = new MergingResults(cardsMerge, assignmentsMerge, peopleMerge, commentsMerge, cardListMerge);

        return result;
    }

    private MergeLocalRemote<CardList> MergeCardLists()
    {
        var m = new MergeLocalRemote<CardList>();

        var localDict = localData.CardLists.ToDictionary(i => i.Id);
        var remoteDict = remoteData.CardLists.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                m.Local.Created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value) m.Local.Updated.Add(kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                m.Local.Deleted.Add(kvp.Value);
            }
        }

        return m;
    }

    private MergeLocalRemote<Person> MergePeople()
    {
        var m = new MergeLocalRemote<Person>();

        var localDict = localData.People.ToDictionary(i => i.Id);
        var remoteDict = remoteData.People.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                m.Local.Created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value) m.Local.Updated.Add(kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                m.Local.Deleted.Add(kvp.Value);
            }
        }

        return m;
    }

    private MergeLocalRemote<Card> MergeCards()
    {
        var m = new MergeLocalRemote<Card>();

        var localDict = localData.Cards.ToDictionary(i => i.Id);
        var remoteDict = remoteData.Cards.ToDictionary(i => i.Id);

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                m.Local.Created.Add(kvp.Value);
            }
            else
            {
                //Updates
                if (localDict[kvp.Key] != kvp.Value)
                {
                    //TODO
                    //proper updates
                    m.Local.Updated.Add(kvp.Value);
                }
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                m.Local.Deleted.Add(kvp.Value);
            }
        }

        return m;
    }

    private MergeLocalRemote<Comment> MergeComments()
    {
        var m = new MergeLocalRemote<Comment>();

        var localDict = localData.Comments.ToDictionary(i => i.CardId);
        var remoteDict = remoteData.Comments.ToDictionary(i => i.CardId);

        foreach (var kvp in remoteDict)
        {
            //First comment on card, made remotely
            if (!localDict.ContainsKey(kvp.Key))
            {
                m.Local.Created.Add(kvp.Value);
            }
            else
            {
                if (localDict[kvp.Key].Id == kvp.Value.Id)
                {
                    //Edited comment, keep Trello version
                    if (localDict[kvp.Key] != kvp.Value) m.Local.Updated.Add(kvp.Value);
                }
                else
                {
                    //Keep newest comment, delete older one
                    if (localDict[kvp.Key].CreatedAt > kvp.Value.CreatedAt)
                    {
                        //Keep local comment, create it on Trello, delete Trello comment both locallly and on Trello
                        m.Remote.Created.Add(localDict[kvp.Key]);
                        m.Local.Deleted.Add(kvp.Value);
                        m.Remote.Deleted.Add(kvp.Value);
                    }
                    else
                    {
                        //Keep Trello comment, create it locally, delete local comment both locallly and on Trello
                        m.Local.Created.Add(kvp.Value);
                        m.Local.Deleted.Add(localDict[kvp.Key]);
                        m.Remote.Deleted.Add(localDict[kvp.Key]);
                    }
                }
            }
        }

        foreach (var kvp in localDict)
        {
            //First comment on card, made locally
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                //Post comment on Trello
                m.Remote.Created.Add(kvp.Value);
            }
        }

        return m;
    }

    private MergeLocalRemote<Assignment> MergeAssignments()
    {
        var m = new MergeLocalRemote<Assignment>();

        var localDict = localData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));
        var remoteDict = remoteData.Assignments.ToDictionary(i => (i.CardId, i.PersonId));

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                m.Local.Created.Add(kvp.Value);
            }
            //Else assignment already exists, and there is no need to update it
        }
        foreach (var kvp in localDict)
        {
            //Deletions
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                m.Local.Deleted.Add(kvp.Value);
            }
        }

        return m;
    }

}
