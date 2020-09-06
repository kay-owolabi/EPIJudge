using System;
using System.IO;

namespace epi.TestFramework
{
    public class TestUtils
    {
        public static string GetDefaultTestDataDirPath()
        {
            const int MAX_SEARCH_DEPTH = 4;
            const string DIR_NAME = "test_data";

            string path = Environment.CurrentDirectory;
            for (int i = 0; i < MAX_SEARCH_DEPTH; i++)
            {
                if (Directory.Exists(Path.Combine(path, DIR_NAME)))
                {
                    return Path.Combine(path, DIR_NAME);
                }
                path = Path.GetDirectoryName(path);
                if (string.IsNullOrWhiteSpace(path))
                {
                    break;
                }
            }

            throw new Exception("Can't find test data directory. Please start the program with \"--test_data_dir <path>\" command-line option");
        }

        public static string GetFilePathInJudgeDir(string filename)
        {
            return Path.Combine(Path.GetDirectoryName(GetDefaultTestDataDirPath()), filename);
        }
    }
}
