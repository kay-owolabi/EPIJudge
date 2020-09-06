using System;
using System.Collections.Generic;
using System.Linq;
using epi.TestFramework;

namespace epi
{
    public static class AbsentValueArray
    {
        public static int FindMissingElement(IEnumerable<int> stream)
        {
            // TODO - you fill in here.
            return 0;
        }

        [EpiTest("absent_value_array.tsv")]
        public static void FindMissingElementWrapper(List<int> stream)
        {
            try
            {
                int res = FindMissingElement(stream);
                var x = stream.Where(a => a.Equals(res))?.First();
                if (stream.Where(a => a.Equals(res))?.First() != null)
                {
                    throw new TestFailure(res.ToString() + " appears in stream");
                }
            }
            catch (ArgumentException e)
            {
                throw new TestFailure("Unexpected no missing element exception");
            }
        }

        public static void Main(string[] args)
        {
        }
    }
}
