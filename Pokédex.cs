using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace pokemon.net
{
    public class Pokédex
    {
        private List<Pokémon> all;
        private Dictionary<int, Pokémon[]> map;

        public Pokédex(string fileName)
        {
            all = new List<Pokémon>();
            map = new Dictionary<int, Pokémon[]>();

            var pokémonList = File.ReadAllText(fileName);
            dynamic allPokemon = JsonConvert.DeserializeObject(pokémonList);
            foreach (var pokémon in allPokemon)
            {
                var newPokémon = new Pokémon(pokémon.Value);
                all.Add(newPokémon);
                foreach (var chr in newPokémon.UniqueChars())
                {
                    if (map.ContainsKey(chr))
                    {
                        map[chr] = map[chr].Concat(new[] {newPokémon}).ToArray();
                    }
                    else
                    {
                        map[chr] = new[] {newPokémon};
                    }
                }
            }
            var newMap = new Dictionary<int, Pokémon[]>();
            foreach (var kvp in map.OrderBy(m => m.Value.Count()))
            {
                newMap[kvp.Key] = kvp.Value.OrderBy(pok => pok.Usefulness(CharsOrderMap())).ToArray();
            }
            map = newMap;
        }

        public List<Pokémon> All()
        {
            return all;
        }

        public Dictionary<int, Pokémon[]> Map()
        {
            return map;
        }

        private int[] charsOrder;
        public int[] CharsInOrder()
        {
            if (charsOrder == null)
            {
                charsOrder = map.OrderBy(m => m.Value.Count()).Select(kvp => kvp.Key).ToArray();
            }
            return charsOrder;
        }

        private Dictionary<int, int> charsOrderMap;
        private Dictionary<int, int> CharsOrderMap()
        {
            if (charsOrderMap == null)
            {
                charsOrderMap = CharsInOrder().Select((value, index) => new {value, index}).ToDictionary(x => x.value, x => x.index);
            }
            return charsOrderMap;
        }
    }
}