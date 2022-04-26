namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;

public static class CardListLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<CardList> merge, CardList local)
    {
        //Delete local entry
        merge.Local.Deleted.Add(local);

    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<CardList> merge, CardList remote)
    {
        //Mirror remote entry locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<CardList> merge, CardList local, CardList remote)
    {
        //Replace local with remote if they differ
        if (local != remote)
        {
            merge.Local.Updated.Add(remote);
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

