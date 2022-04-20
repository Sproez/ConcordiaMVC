namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CardListLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<CardList> merge, CardList local)
    {
        //Should never happen
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
}

