namespace ConcordiaLib.Collections;

public class MergeCUD<T>
{
    public List<T> Created { get; init; }
    public List<T> Updated { get; init; }
    public List<T> Deleted { get; init; }

    public MergeCUD(List<T>? created = null, List<T>? updated = null, List<T>? deleted = null)
    {
        Created = created ?? new List<T>();
        Updated = updated ?? new List<T>();
        Deleted = deleted ?? new List<T>();
    }
}
