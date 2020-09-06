using System;
using System.Collections.Generic;
using System.Text.Json;

namespace epi.TestFramework.SerializationTraits
{
    public class BooleanTrait : SerializationTrait
    {
        public override IList<string> GetMetricNames(string argName)
        {
            return new string[0];
        }

        public override IList<int> GetMetrics(object x)
        {
            if (x.GetType().Equals(typeof(bool)))
            {
                return new int[0];
            } else
            {
                throw new Exception("Expected Boolean");
            }
        }

        public override string Name()
        {
            return "bool";
        }

        public override object Parse(JsonElement jsonObject)
        {
            return jsonObject.GetBoolean();
        }
    }
}
