namespace ConcordiaMerger.MergeLogic;

using System;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;

public static class AssignmentLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<Assignment> merge, Assignment local)
    {
        //Should never happen
        //Log and delete the local entry
        //TODO logging
        merge.Local.Deleted.Add(local);
    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<Assignment> merge, Assignment remote)
    {
        //Create entry locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<Assignment> merge, Assignment local, Assignment remote)
    {
        //Should never happen
        if (local != remote)
        {
            //TODO logging
        }
    }

    public static (
        Action<MergeLocalRemote<Assignment>, Assignment>,
        Action<MergeLocalRemote<Assignment>, Assignment>,
        Action<MergeLocalRemote<Assignment>, Assignment, Assignment>
        ) 
        GetMergeActions() => (MergeWhenOnlyLocal, MergeWhenOnlyRemote, MergeWhenConflict);
}

