namespace Bestemmie_per_Igor
{
    public class In1
    {
        public string Test { get; set; } = null!;
        public In2 Nested { get; set; } = null!;
    }

    public class In2
    {
        public int AAAA { get; set; }
    }

    public record Out
        (
        string MyTest,
        int MyInt
        )
    {

    }
}
