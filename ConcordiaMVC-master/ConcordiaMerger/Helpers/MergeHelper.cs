using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Collections;

namespace ConcordiaMerger.Helpers;

public static class MergeHelper
{
    public static MergeLocalRemote<T> Merge<T, Tkey>(
#pragma warning disable CS8714 // supress Type Warning on Tkey
    Dictionary<Tkey, T> localDict, Dictionary<Tkey, T> remoteDict,
#pragma warning restore CS8714 
    //Tuple with 3 Actions in it
    (Action<MergeLocalRemote<T>, T> MergeWhenOnlyLocal,
    Action<MergeLocalRemote<T>, T> MergeWhenOnlyRemote,
    Action<MergeLocalRemote<T>, T, T> MergeWhenConflict) tuple
    )
    {
        var m = new MergeLocalRemote<T>();

        foreach (var kvp in remoteDict)
        {
            //Creation
            if (!localDict.ContainsKey(kvp.Key))
            {
                tuple.MergeWhenOnlyRemote(m, kvp.Value);
            }
            else
            {
                //Update
                tuple.MergeWhenConflict(m, localDict[(Tkey)kvp.Key], kvp.Value);
            }
        }
        foreach (var kvp in localDict)
        {
            //Deletion
            if (!remoteDict.ContainsKey(kvp.Key))
            {
                tuple.MergeWhenOnlyLocal(m, kvp.Value);
            }
        }

        return m;
    }
}

