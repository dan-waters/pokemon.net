using System.Linq;

namespace pokemon.net
{
    public class Pokéset
    {
        private Pokémon[] set;

        public Pokéset(Pokémon[] set)
        {
            this.set = set;
        }

        public Pokémon[] Set()
        {
            return set;
        }

        public bool IsLessThanOrEqualTo(Pokéset other)
        {
            if (Length() < other.Length())
            {
                return true;
            }
            if (Length() == other.Length())
            {
                return TotalLength() <= other.TotalLength();
            }
            return false;
        }
        
        public bool IsLessThan(Pokéset other)
        {
            if (Length() < other.Length())
            {
                return true;
            }
            if (Length() == other.Length())
            {
                return TotalLength() < other.TotalLength();
            }
            return false;
        }

        private int code;
        public int Code()
        {
            if (code == 0)
            {
                foreach (var pokémon in set)
                {
                    code |= pokémon.Code();
                }
            }
            return code;
        }

        private int? length;
        public int Length()
        {
            if (length == null)
            {
                length = set.Count();
            }
            return length.Value;
        }

        private int[] chars;
        public int[] Chars()
        {
            if (chars == null)
            {
                chars = set.SelectMany(s => s.UniqueChars()).Distinct().ToArray();
            }
            return chars;
        }

        private int? totalLength;
        public int TotalLength()
        {
            if (totalLength == null)
            {
                totalLength = set.SelectMany(s => s.Name()).Count();
            }
            return totalLength.Value;
        }

        private int? missingCharCount;
        public int MissingCharCount()
        {
            if (missingCharCount == null)
            {
                missingCharCount = 26 - Chars().Length;
            }
            return missingCharCount.Value;
        }

        public override string ToString()
        {
            return string.Format("{0} for a total length of {1} pokémon and {2} characters", string.Join(", ", set.Select(s => s.ToString())), Length(), TotalLength());
        }
    }
}