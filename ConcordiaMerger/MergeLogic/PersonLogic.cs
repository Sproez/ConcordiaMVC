﻿namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<Person> merge, Person local)
    {
        //Should never happen
        //Log and delete the local entry
        //TODO logging
        merge.Local.Deleted.Add(local);
    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<Person> merge, Person remote)
    {
        //Create entry locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<Person> merge, Person local, Person remote)
    {
        //Replace local with remote if they differ
        if (local != remote)
        {
            merge.Local.Updated.Add(remote);
        }
        //Do nothing if they are equal (we're already synced)
    }
}

