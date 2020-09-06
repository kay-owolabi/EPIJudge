using System;
using System.Collections.Generic;
using System.Text.Json;

namespace epi.TestFramework.SerializationTraits
{
    public abstract class SerializationTrait
    {
        public abstract string Name();
        public abstract object Parse(JsonElement jsonObject);
        public abstract IList<string> GetMetricNames(string argName);
        public abstract IList<int> GetMetrics(object x);
        public bool IsVoid() { return false; }
    }
}
