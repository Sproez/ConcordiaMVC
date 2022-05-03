namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;

public static class CommentLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<Comment> merge, Comment local)
    {
        //Mirror local comment remotely
        merge.Remote.Created.Add(local);
    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<Comment> merge, Comment remote)
    {
        //Mirror remote comment locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<Comment> merge, Comment local, Comment remote)
    {
        //If the comments have different Id, keep the most recent and delete the other
        if (local.Id != remote.Id)
        {
            if (local.CreatedAt > remote.CreatedAt)
            {
                merge.Remote.Created.Add(local);
                merge.Remote.Deleted.Add(remote);
            }
            else
            {
                merge.Local.Created.Add(remote);
                merge.Local.Deleted.Add(local);
                //If the local comment has a local-only Id, there is no need to remove it from remote
                if (!local.Id.StartsWith("local")) merge.Remote.Deleted.Add(local);
            }
        }
        //If the comments have the same Id and are different, use the Trello version
        else if (local != remote)
        {
            merge.Local.Updated.Add(remote);
        }

    }

    public static (
        Action<MergeLocalRemote<Comment>, Comment>,
        Action<MergeLocalRemote<Comment>, Comment>,
        Action<MergeLocalRemote<Comment>, Comment, Comment>
        )
        GetMergeActions() => (MergeWhenOnlyLocal, MergeWhenOnlyRemote, MergeWhenConflict);

}

