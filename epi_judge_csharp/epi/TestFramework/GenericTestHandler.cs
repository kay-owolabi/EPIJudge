using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using epi.TestFramework.SerializationTraits;

namespace epi.TestFramework
{
    public class GenericTestHandler
    {
        private MethodInfo func;
        private IList<Type> paramTypes;
        private bool hasExecutorHook;
        private IList<SerializationTrait> paramTraits;
        private IList<String> paramNames;
        private SerializationTrait retValueTrait;
        private Func<object, object, bool> comp;
        private bool customExpectedType;

        /**
         * This constructor initializes type parsers for all arguments and return type
         * of func.
         *
         * @param func         - a method to test.
         * @param comp         - an optional comp for result. If comp is null, values
         *                       are compared with equals().
         * @param expectedType - can be used with a custom comp that has different
         *                       types for expected and result arguments.
         */
        public GenericTestHandler(MethodInfo func, Func<Object, Object, bool> comp,
                                  FieldInfo expectedType)
        {
            this.func = func;
            this.comp = comp;
            hasExecutorHook = false;
            paramTypes = func.GetParameters().Select(p => p.ParameterType).ToList();//List.of(func.paGetGenericParameterTypes());

            if (paramTypes.Count >= 1 &&
                paramTypes[0].Equals(typeof(TimedExecutor)))
            {
                hasExecutorHook = true;
                paramTypes.RemoveAt(0);
            }

            if (paramTypes.Count >= 1 && paramTypes[0].Equals(typeof(TestTimer)))
            {
                throw new Exception("This program uses deprecated TestTimer hook");
            }

            paramTraits = paramTypes.stream()
                                  .map(TraitsFactory::getTrait)
                                  .collect(Collectors.toList());
            paramNames = Arrays.stream(func.getParameters())
                                 .map(Parameter::getName)
                                 .collect(Collectors.toList());
            if (hasExecutorHook)
            {
                paramNames.RemoveAt(0);
            }

            if (expectedType == null)
            {
                retValueTrait = TraitsFactory.GetTrait(func.ReturnType);
            }
            else
            {
                retValueTrait = TraitsFactory.GetTrait(expectedType.getGenericType());
            }

            customExpectedType = expectedType != null;
        }

        /**
         * This method ensures that test data header matches with the signature
         * of the method provided in constructor.
         *
         * @param signature - the header from a test data file.
         */
        public void ParseSignature(List<String> signature)
        {
            if (signature.Count != paramTypes.Count + 1)
            {
                throw new Exception("Signature parameter count mismatch");
            }

            for (int i = 0; i < paramTypes.Count; i++)
            {
                MatchTypeNames(paramTraits[i].Name(), signature[i],
                               String.Format("{0} argument", i));
            }

            if (!customExpectedType)
            {
                MatchTypeNames(retValueTrait.Name(), signature[signature.Count - 1],
                               "Return value");
            }
        }

        private void MatchTypeNames(String expected, String fromTestData,
                                    String sourceName)
        {
            if (!expected.Equals(TestUtils.filterBracketComments(fromTestData)))
            {
                throw new Exception(
                    String.Format("%s type mismatch: expected %s, got %s", sourceName,
                                  expected, fromTestData));
            }
        }

        /**
         * This method is invoked for each row in a test data file (except the
         * header). It deserializes the list of arguments and calls the user method
         * with them.
         *
         * @param timeoutSeconds - number of seconds to timeout.
         * @param metricsOverride -
         * @param testArgs - serialized arguments.
         * @return array, that contains [result of comparison of expected and result,
         * expected, result]. Two last entries are omitted in case of the void return
         * type
         */
        public TestOutput RunTest(
            long timeoutSeconds,
            Func<List<int>, List<Object>, List<int>, bool> metricsOverride,
            List<String> testArgs)
        {
            try
            {
                int expectedParamCount = paramTraits.Count + (ExpectedIsVoid() ? 0 : 1);
                if (testArgs.Count != expectedParamCount)
                {
                    throw new Exception(
                        String.Format("Invalid argument count: expected %d, actual %d",
                                      expectedParamCount, testArgs.Count));
                }

                IList<Object> parsed = new List<object>();
                for (int i = 0; i < paramTraits.Count; i++)
                {
                    parsed.Add(paramTraits[i].Parse(Json.Parse(testArgs[i])));
                }
                IList<int> metrics = calculateMetrics(parsed);
                metrics = metricsOverride.apply(metrics, parsed);

                Object result;
                TimedExecutor executor = new TimedExecutor(timeoutSeconds);

                if (hasExecutorHook)
                {
                    parsed.Add(0, executor);
                    result = func.Invoke(null, parsed.ToArray());
                }
                else
                {
                    result = executor.Run(()->func.invoke(null, parsed.toArray()));
                }

                if (!ExpectedIsVoid())
                {
                    Object expected =
                        retValueTrait.parse(Json.parse(testArgs.get(testArgs.Count - 1)));
                    AssertResultsEqual(expected, result);
                }

                return new TestOutput(executor.getTimer(), metrics);
            }
            catch (IllegalAccessException e)
            {
                throw new Exception(e.getMessage());
            }
            catch (InvocationTargetException e)
            {
                Throwable t = e.getCause();
                if (t.GetType() == typeof(Exception))
                {
                    throw (Exception)t;
                }
                else if (t.GetType() == typeof(Error)) {
                    throw (Error)t;
                } else
                {
                    // Improbable except for intended attempts to break the code, but anyway
                    throw new Exception(t);
                }
            }
        }


        private void AssertResultsEqual(Object expected, Object result)
        {
            bool comparisonResult;
            if (comp != null)
            {
                comparisonResult = comp.Invoke(expected, result);
            }
            else if (expected == null)
            {
                comparisonResult = result == null;
            }
            else if (expected.GetType() == typeof(float) && result.GetType() == typeof(float))
            {
                comparisonResult =
                    TestUtils.FloatComparison((float)expected, (float)result);
            }
            else if (expected.GetType() == typeof(Double) && result.GetType() == typeof(Double))
            {
                comparisonResult =
                    TestUtils.DoubleComparison((Double)expected, (Double)result);
            }
            else if (expected.GetType() == typeof(TreeLike<T, T>) && result.GetType() == typeof(TreeLike<T, T>))
            {
                BinaryTreeUtils.assertEqualBinaryTrees((TreeLike<Object, T>)expected,
                                                       (TreeLike<Object, T>)result);
                return;
            }
            else
            {
                comparisonResult = expected.Equals(result);
            }
            if (!comparisonResult)
            {
                throw new TestFailure()
                    .WithProperty(TestFailure.PropertyName.EXPECTED, expected)
                    .WithProperty(TestFailure.PropertyName.RESULT, result);
            }
        }

        public IList<String> MetricNames()
        {
            return IntStream.range(0, Math.min(paramTraits.Count, paramNames.Count))
                .mapToObj(i->paramTraits.get(i).getMetricNames(paramNames.get(i)))
                .flatMap(Collection::stream)
                .collect(Collectors.toList());
        }

        private IList<int> calculateMetrics(List<Object> params)
        {
            return IntStream.range(0, Math.min(paramTraits.Count, params.Count))
                .mapToObj(i->paramTraits.get(i).getMetrics(params.get(i)))
                .flatMap(Collection::stream)
                .collect(Collectors.toList());
        }

        public bool ExpectedIsVoid() { return retValueTrait.isVoid(); }

        public IList<String> ParamNames() { return paramNames; }
    }
}
