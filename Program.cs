using System;
using System.Linq;
using System.Threading.Tasks;

namespace pokemon.net
{
    class Program
    {
        private static Pokédex pokédex;
        private static Pokéset minPokéset;
        private static DateTime time;
        private static int ALPHABET = (int)(Math.Pow(2, 26) - 1);

        static void Main(string[] args)
        {
            time = DateTime.Now;

            pokédex = new Pokédex(args.Any() ? args[0] : @"..\..\pokemon.txt");

            minPokéset = new Pokéset(pokédex.All().ToArray());

            Parallel.ForEach(pokédex.Map()[pokédex.CharsInOrder()[0]], pokémon =>
            {
                var set = new Pokéset(new[] {pokémon});
                if (set.Code() < ALPHABET)
                {
                    AddNextToSet(set);
                }
            });

            Console.WriteLine("Found min_set: {0}. Completed in {1} seconds", minPokéset, (DateTime.Now - time).TotalSeconds);
        }

        static void AddNextToSet(Pokéset set)
        {
            foreach (var pokémon in pokédex.Map()[pokédex.CharsInOrder().Except(set.Chars()).First()])
            {
                var newSet = new Pokéset(set.Set().Concat(new[] {pokémon}).ToArray());
                if (newSet.Code() == ALPHABET)
                {
                    if (newSet.IsLessThanOrEqualTo(minPokéset))
                    {
                        minPokéset = newSet;
                        Console.WriteLine("After {0} seconds, found new min_set: {1}", (DateTime.Now - time).TotalSeconds, minPokéset);
                    }
                }
                else if (CanAddTwoMore(newSet) || CanAddOneMore(newSet))
                {
                    AddNextToSet(newSet);
                }
            }
        }

        static bool CanAddTwoMore(Pokéset set)
        {
            return set.Length() + 1 < minPokéset.Length();
        }

        private static bool CanAddOneMore(Pokéset newSet)
        {
            return (newSet.Length() + 1 == minPokéset.Length() && newSet.TotalLength() + newSet.MissingCharCount() <= minPokéset.TotalLength());
        }
    }
}
