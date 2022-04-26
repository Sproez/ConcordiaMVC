namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;

public static class CardListLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<CardList> merge, CardList local)
    {
        //TODO mirror local entry remotely
    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<CardList> merge, CardList remote)
    {
        //Mirror remote entry locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<CardList> merge, CardList local, CardList remote)
    {
        //Replace remote with local if they differ
        if (local != remote)
        {
            merge.Remote.Updated.Add(local);
        }
        //Do nothing if they are equal (we're already synced)
    }

    public static (
        Action<MergeLocalRemote<CardList>, CardList>,
        Action<MergeLocalRemote<CardList>, CardList>,
        Action<MergeLocalRemote<CardList>, CardList, CardList>
        )
        GetMergeActions() => (MergeWhenOnlyLocal, MergeWhenOnlyRemote, MergeWhenConflict);
}

