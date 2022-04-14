using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestemmie_per_Igor
{
    public class Source
    {
        public string Title { get; init; }
        public string Memes { get; init; }

        public string Id { get; init; }
    }

    public record Destination
    (
    string Title,
    int MoreMemes
    )
    {
        public string Id { get; init; }
    }
}
