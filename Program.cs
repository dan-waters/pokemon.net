using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemon.net
{
    class Program
    {
        private static Pokédex pokédex;
        private static Pokéset minPokéset;
        private static int ALPHABET = (int)(Math.Pow(2, 26) - 1);

        static void Main(string[] args)
        {
            var time = DateTime.Now;

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
                        Console.WriteLine("Found new min_set: {0}", minPokéset);
                    }
                }
                else if (newSet.Length() + 1 < minPokéset.Length() || (newSet.Length() + 1 == minPokéset.Length() && newSet.TotalLength() + newSet.MissingCharCount() <= minPokéset.TotalLength()))
                {
                    AddNextToSet(newSet);
                }
            }
        }
    }
}
