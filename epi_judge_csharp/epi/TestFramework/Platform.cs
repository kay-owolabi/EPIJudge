using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
namespace epi.TestFramework
{
    public class Platform
    {

        private static bool? isWindows;
        private static bool? is64Bit;
        private static TriBool dllLoaded = TriBool.INDETERMINATE;
        private static bool enableTtyOutput = false;
        private static bool enableColorOutput = false;

        public static void stdOutClearLine() { Console.Write("\r"); }

        public static void setOutputOpts(TriBool ttyMode, TriBool colorMode)
        {
            enableTtyOutput = ttyMode.GetOrDefault(true); // System.Console() != null
            enableColorOutput = colorMode.GetOrDefault(enableTtyOutput);
            InitColorOutput();
        }

        public static bool UseTtyOutput() { return enableTtyOutput; }

        public static bool UseColorOutput() { return enableColorOutput; }

        public static bool RunningOnWin()
        {
            if (isWindows == null)
            {
                isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);//System.getProperty("os.name").startsWith("Windows");
            }
            return (bool)isWindows;
        }

        public static bool RunningOn64BitVM()
        {
            if (is64Bit == null)
            {
                is64Bit = Environment.Is64BitOperatingSystem;
            }
            return (bool)is64Bit;
        }

        private static void InitColorOutput()
        {
            if (RunningOnWin() && UseColorOutput() &&
                dllLoaded == TriBool.INDETERMINATE)
            {
                String dllName =
                    RunningOn64BitVM() ? "console_color_64" : "console_color_32";

                try
                {
                    Assembly.LoadFile(dllName);
                    dllLoaded = TriBool.TRUE;
                }
                catch (FileLoadException ex)
                {
                    dllLoaded = TriBool.FALSE;
                    Console.Write(
                        "Warning: {0}.dll was not found. Colored output is disabled.\n"
                            +
                            "In order to enable it, pass -Djava.library.path=<path to EPIJudge>/epi_judge_csharp/epi/test_framework option to csharp.\n",
                        dllName);
                }
            }
        }

        public static int WinSetConsoleTextAttribute(int attr)
        {
            if (dllLoaded == TriBool.TRUE)
            {
                return WinSetConsoleTextAttributeImpl(attr);
            }
            return 0;
        }

        /**
         * Interface to the native wrapper of WinAPI.
         * Set CONSOLE_SCREEN_BUFFER_INFO.wAttributes to attr for the stdout handle.
         * @param attr - new value for wAttributes
         * @return previous value of wAttributes
         */
        private static int WinSetConsoleTextAttributeImpl(int attr) { return 0; }
    }
}

