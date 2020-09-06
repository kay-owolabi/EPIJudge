using System;
using System.Text.Json;

namespace epi.TestFramework
{
    public class EpiUserType
    {
        public Type[] CtorParams { get; private set; }
        public EpiUserType(Type[] ctorParams)
        {
            CtorParams = ctorParams;
        }
    }
}
