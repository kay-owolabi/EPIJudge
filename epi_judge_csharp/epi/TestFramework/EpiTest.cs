using System;

namespace epi.TestFramework
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EpiTest : Attribute
    {
        public string TestDataFile { get; private set; }
        public EpiTest(string testDataFile)
        {
            TestDataFile = testDataFile;
        }
    }
}
