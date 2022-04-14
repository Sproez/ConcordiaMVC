using AutoMapper;

namespace Bestemmie_per_Igor
{
	public class CustomResolver : IValueResolver<Source, Destination, int>
	{
		private readonly int _b;

		public CustomResolver(int bonus) {
			_b = bonus;
		}

		public int Resolve(Source source, Destination destination, int member, ResolutionContext context)
		{
			return source.Memes.Length;
		}
	}
	
	public static class PINGAS
    {
		public static int cancer(string a) {
			return a.Length * 3;
		}

    }
}
