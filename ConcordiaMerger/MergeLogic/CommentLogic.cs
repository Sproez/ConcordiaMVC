namespace ConcordiaMerger.MergeLogic;

using ConcordiaLib.Domain;
using ConcordiaLib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //If the comments have different Id, keep the newest one and delete the other
        if (local.Id != remote.Id)
        {
            if (local.CreatedAt > remote.CreatedAt) {
                merge.Remote.Created.Add(local);
                merge.Local.Deleted.Add(remote);
                merge.Remote.Deleted.Add(remote);
            } else {
                merge.Local.Created.Add(remote);
                merge.Local.Deleted.Add(local);
                merge.Remote.Deleted.Add(local);
            }
        }
        //If the comments have the same Id, do nothing
        //TODO consider edited comments
    }
}

