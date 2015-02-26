using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace pokemon.net
{
    public class Pokémon
    {
        private string name;
        private int[] uniqueChars;
        private int[] chars;

        public Pokémon(string name)
        {
            this.name = name.ToLowerInvariant();
            chars = name.Replace("-", "").Replace("2", "").Select(chr => (int)Math.Pow(2, chr - 'a')).ToArray();
            uniqueChars = chars.Distinct().ToArray();
        }

        public string Name()
        {
            return name;
        }

        public int[] UniqueChars()
        {
            return uniqueChars;
        }

        public int[] Chars()
        {
            return chars;
        }

        public int Length()
        {
            return Name().Length;
        }

        private int code;
        public int Code()
        {
            if (code == 0)
            {
                foreach (var uniqueChar in UniqueChars())
                {
                    code |= uniqueChar;
                }
            }
            return code;
        }

        private int? usefulness;
        public int Usefulness(Dictionary<int, int> order)
        {
            if (usefulness == null)
            {
                usefulness = Chars().Sum(c => order[c]);
            }
            return usefulness.Value;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Code: {1}", Name(), Code());
        }
    }
}