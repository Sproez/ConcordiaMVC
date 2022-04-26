namespace Bestemmie_per_Igor
{
    public class Source
    {
        public string Title { get; init; } = null!;
        public string Memes { get; init; } = null!;

        public string Id { get; init; } = null!;
    }

    public record Destination
    (
    string Title,
    int MoreMemes
    )
    {
        public string Id { get; init; } = null!;
    }
}
