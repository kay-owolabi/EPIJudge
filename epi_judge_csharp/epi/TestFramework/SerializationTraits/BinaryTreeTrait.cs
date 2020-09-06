using System;
using System.Collections.Generic;
using System.Text.Json;

namespace epi.TestFramework.SerializationTraits
{
    public class BinaryTreeTrait : SerializationTrait
    {
        private Type nodeType;
        private SerializationTrait innerTypeTrait;

        public BinaryTreeTrait(Type nodeType, SerializationTrait innerTypeTrait)
        {
            this.nodeType = nodeType;
            this.innerTypeTrait = innerTypeTrait;
        }

        public override string ToString()
        {
            return string.Format("binary_tree({0})", innerTypeTrait.Name());
        }

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
