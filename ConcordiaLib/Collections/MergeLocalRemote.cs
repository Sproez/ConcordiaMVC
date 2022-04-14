namespace ConcordiaLib.Collections;

public class MergeLocalRemote<T>
{
    public MergeCUD<T> Local { get; init; }
    public MergeCUD<T> Remote { get; init; }

    public MergeLocalRemote(MergeCUD<T>? local = null, MergeCUD<T>? remote = null)
    {
        Local = local ?? new MergeCUD<T>();
        Remote = remote ?? new MergeCUD<T>();
    }
}
