using ConcordiaLib.Collections;
using ConcordiaLib.Domain;

namespace ConcordiaMerger.MergeLogic;


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

    public static (
        Action<MergeLocalRemote<Person>, Person>,
        Action<MergeLocalRemote<Person>, Person>,
        Action<MergeLocalRemote<Person>, Person, Person>
        )
        GetMergeActions() => (MergeWhenOnlyLocal, MergeWhenOnlyRemote, MergeWhenConflict);

}

