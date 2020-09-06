using System;
using System.Collections.Generic;
using System.IO;

namespace epi.TestFramework
{
    /// <summary>
    /// This class contains parameters that control test execution
    /// </summary>
    public class TestConfig
    {
        /**
         * Path to directory with .tsv files
         */
        public string testDataDir;
        /**
         * Program source filename
         */
        public string testFile;
        /**
         * Name of corresponding .tsv file
         */
        public string testDataFile;
        /**
         * If TRUE, enable advanced output (mainly usage of \r)
         * If INDETERMINATE, try to autodetect if output is console
         */
        public TriBool ttyMode;
        /**
         * If TRUE, enable colored output
         * If INDETERMINATE, try to autodetect if output is console
         */
        public TriBool colorMode;
        /**
         * If True, update problem_mapping.js
         */
        public bool updateJs;
        /**
         * If > 0, run each test with a timeout
         */
        public long timeoutSeconds;
        /**
         * Number of failures, before the testing is terminated
         * If zero, testing is never terminated
         */
        public int numFailedTestsBeforeStop;
        /**
         * If True, enable solution complexity analyze
         */
        public bool analyzeComplexity;
        /**
         * If > 0, calculate complexity with timeout
         */
        public long complexityTimeout;
        /**
         * Function for adjusting list of metric names
         * By default identity function
         * Another function may be set in programConfig callback
         */
        public Func<IList<String>, IList<String>> metricNamesOverride;
        /**
         * Function for adjusting list of generated metrics
         * All changes should be isomorphic to metricNamesOverride
         * By default returns identical metrics list
         * Another function may be set in programConfig callback
         */
        public Func<IList<int>, IList<object>, IList<int>> metricsOverride;

        public TestConfig(string testFile, string testDataFile, long timeoutSeconds,
                    int numFailedTestsBeforeStop)
        {
            this.testFile = testFile;
            this.testDataFile = testDataFile;
            this.ttyMode = TriBool.INDETERMINATE;
            this.colorMode = TriBool.INDETERMINATE;
            this.updateJs = true;
            this.timeoutSeconds = timeoutSeconds;
            this.numFailedTestsBeforeStop = numFailedTestsBeforeStop;
            this.analyzeComplexity = false;
            this.complexityTimeout = 20;
            this.metricNamesOverride = (names) => names;
            this.metricsOverride = (metrics, funcArgs) => metrics;
        }

        public static TestConfig FromCommandLine(String testFile, String testDataFile,
                                           long timeoutSeconds,
                                           int numFailedTestsBeforeStop,
                                           String[] commandlineArgs)
        {
            // Set numFailedTestsBeforeStop to 0, means users want to run as many as
            // tests in one run.
            if (numFailedTestsBeforeStop == 0)
            {
                numFailedTestsBeforeStop = int.MaxValue;
            }
            TestConfig config = new TestConfig(testFile, testDataFile, timeoutSeconds,
                                       numFailedTestsBeforeStop);

            for (int i = 0; i < commandlineArgs.Length; i++)
            {
                switch (commandlineArgs[i])
                {
                    case "--test-data-dir":
                        config.testDataDir = GetParam(commandlineArgs, ++i, "--test-data-dir");
                        break;
                    case "--force-tty":
                        config.ttyMode = TriBool.TRUE;
                        break;
                    case "--no-tty":
                        config.ttyMode = TriBool.FALSE;
                        break;
                    case "--force-color":
                        config.colorMode = TriBool.TRUE;
                        break;
                    case "--no-color":
                        config.colorMode = TriBool.FALSE;
                        break;
                    case "--no-update-js":
                        config.updateJs = false;
                        break;
                    case "--no-complexity":
                        config.analyzeComplexity = false;
                        break;
                    case "--help":
                    case "-h":
                        PrintUsageAndExit();
                        break;
                    default:
                        throw new Exception("CL: Unrecognized argument: " +
                                                   commandlineArgs[i]);
                }
            }

            if (string.IsNullOrWhiteSpace(config.testDataDir) && !string.IsNullOrWhiteSpace(config.testDataDir))
            {
                if (!Directory.Exists(Path.GetFullPath(config.testDataDir)))
                {
                    throw new Exception(string.Format(
                        "CL: --test_data_dir argument ({0}) is not a directory",
                        config.testDataDir));
                }
            }
            else
            {
                config.testDataDir = TestUtils.GetDefaultTestDataDirPath();
            }

            return config;
        }

        private static void PrintUsageAndExit()
        {
            string usageString =
                "usage: <program name> [-h] [--test-data-dir [TEST_DATA_DIR]]\n"
                +
                "                    [--force-tty] [--no-tty] [--force-color] [--no-color]\n"
                + "\n"
                + "optional arguments:\n"
                +
                "  -h, --help                         show this help message and exit\n"
                + "  --test-data-dir [TEST_DATA_DIR]    path to test_data directory\n"
                +
                "  --force-tty                        enable tty features (like printing output on the same line) even in case stdout is not a tty device\n"
                + "  --no-tty                           never use tty features\n"
                +
                "  --force-color                      enable colored output even in case stdout is not a tty device\n"
                + "  --no-color                         never use colored output\n"
                + "  --no-update-js                     no update problem_mapping.js\n";
            Console.Write(usageString);
            Environment.Exit(0);
        }

        private static string GetParam(string[] commandlineArgs, int i, string argName)
        {
            if (i >= commandlineArgs.Length)
            {
                throw new Exception("CL: Missing parameter for " + argName);
            }
            return commandlineArgs[i];
        }
    }
}
