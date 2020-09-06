using System;
using System.Collections.Generic;

namespace epi.TestFramework.SerializationTraits
{
    public class TraitsFactory
    {
        public static SerializationTrait GetTrait(Type type)
        {
            if (type.Equals(typeof(void)))
            {
                return new VoidTrait();
            }

            if (type.IsGenericType)
            {
                Type ty = type.GetGenericTypeDefinition();
                if(ty == typeof(List<>))
                {
                   // return new ListTrait()
                }
            }

            throw new Exception("Unsupported argument type: " +
                               type.FullName);
        }
    }
}
