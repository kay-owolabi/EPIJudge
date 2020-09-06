using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace epi.TestFramework
{
    public class GenericTest
    {

        public static TestResult RunFromAttributes(string[] commandlineArgs,
            string testFile, Type testClass)
        {
            Func<object, object, bool> comparator = FindCustomComparatorByAnnotation(testClass);
            FieldInfo expectedType = FindCustomExpectedTypeByAnnotation(testClass);
            Action<TestConfig> programConfig = FindProgramConfigByAnnotation(testClass);

            MethodInfo testFunc = FindMethodWithAnnotation(testClass, typeof(EpiTest));
            if (testFunc == null)
            {
                throw new Exception("Missing method with EpiTest annotation");
            }
            Attribute attrib = Attribute.GetCustomAttribute(testFunc, typeof(EpiTest));
            EpiTest epiTest = (EpiTest)attrib;

            return GenericTestMain(commandlineArgs, testFile, epiTest.TestDataFile, testFunc, comparator, expectedType, programConfig);
        }

        private static TestResult GenericTestMain(string[] commandlineArgs, string testFile, string testDataFile, MethodInfo testFunc, Func<object, object, bool> comparator, FieldInfo expectedType, Action<TestConfig> programConfig)
        {
            JsonElement configOverride;
            try
            {
                configOverride = JsonDocument.Parse(File.ReadAllText(TestUtils.GetFilePathInJudgeDir("config.json"))).RootElement;
            }
            catch (IOException)
            {
                throw new Exception("config.json file not found");
            }


            try
            {
                TestConfig config = TestConfig.FromCommandLine(testFile, testDataFile, configOverride.GetProperty("timeoutSeconds").GetInt32(), configOverride.GetProperty("numFailedTestsBeforeStop").GetInt32(), commandlineArgs);
                if (programConfig != null)
                {
                    programConfig.Invoke(config);
                }
                Platform.setOutputOpts(config.ttyMode, config.colorMode);

            }
            catch (Exception ex)
            {

            }
        }

        private static Action<TestConfig> FindProgramConfigByAnnotation(Type testClass)
        {
            MethodInfo m = FindMethodWithAnnotation(testClass, typeof(EpiProgramConfig));
            if (m == null)
            {
                return null;
            }

            return (config) =>
            {
                try
                {
                    m.Invoke(null, new object[] { config });
                }
                catch (Exception ex) when (ex is MethodAccessException || ex is TargetInvocationException)
                {
                    throw new Exception("", ex);
                }
            };
            throw new NotImplementedException();
        }

        private static FieldInfo FindCustomExpectedTypeByAnnotation(Type testClass)
        {
            return FindFieldWithAnnotation(testClass, typeof(EpiTestExpectedType));
        }

        private static FieldInfo FindFieldWithAnnotation(Type testClass, Type annotationClass)
        {
            return testClass.GetFields().
                Where(fieldInfo => fieldInfo.
                GetCustomAttributes(annotationClass, true).Length > 0).
                FirstOrDefault();
        }

        private static Func<object, object, bool> FindCustomComparatorByAnnotation(Type testClass)
        {
            MethodInfo m = FindMethodWithAnnotation(testClass, typeof(EpiTestComparator));
            if (m == null)
            {
                return null;
            }
            return (expected, result) =>
            {
                try
                {
                    return (bool)m.Invoke(null, new object[] { expected, result });
                }
                catch (Exception ex) when (ex is MethodAccessException || ex is TargetInvocationException)
                {
                    throw new Exception("", ex);
                }
            };
        }

        private static MethodInfo FindMethodWithAnnotation(Type testClass, Type annotationClass)
        {
            return testClass.GetMethods().
                Where(methodInfo => methodInfo.
                GetCustomAttributes(annotationClass, true).Length > 0).
                FirstOrDefault();
        }
    }
}
