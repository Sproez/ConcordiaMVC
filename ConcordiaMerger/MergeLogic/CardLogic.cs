namespace ConcordiaMerger.MergeLogic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;

public static class CardLogic
{
    public static void MergeWhenOnlyLocal(MergeLocalRemote<Card> merge, Card local)
    {
        //Should never happen
        //Log and delete the local entry
        //TODO logging
        merge.Local.Deleted.Add(local);
    }

    public static void MergeWhenOnlyRemote(MergeLocalRemote<Card> merge, Card remote)
    {
        //Create entry locally
        merge.Local.Created.Add(remote);
    }

    public static void MergeWhenConflict(MergeLocalRemote<Card> merge, Card local, Card remote)
    {
        //Suffering ahead
        if (local != remote)
        {
            //TODO better merge logic

            //Id is shared
            var id = local.Id;

            //Decided locally
            //Card list
            var list = local.CardListId;
            //Decided remotely
            //Title, description, priority, and due by date 
            var title = remote.Title;
            var desc = remote.Description;
            var priority = remote.Priority;
            var dueby = remote.DueBy;

            var mergedCard = new Card(title, desc, dueby, priority)
            {
                Id = id,
                CardListId = list
            };

            merge.Local.Updated.Add(mergedCard);
            merge.Remote.Updated.Add(mergedCard);
        }
        //Do nothing if they are equal (we're already synced)
    }
}

