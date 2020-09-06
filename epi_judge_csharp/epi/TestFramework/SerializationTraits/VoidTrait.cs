using System;
using System.Collections.Generic;
using System.Text.Json;

namespace epi.TestFramework.SerializationTraits
{
    public class VoidTrait : SerializationTrait
    {
        public override IList<string> GetMetricNames(string argName)
        {
            throw new NotImplementedException();
        }

        public override IList<int> GetMetrics(object x)
        {
            throw new NotImplementedException();
        }

        public override string Name()
        {
            throw new NotImplementedException();
        }

        public override object Parse(JsonElement jsonObject)
        {
            throw new NotImplementedException();
        }
    }
}
