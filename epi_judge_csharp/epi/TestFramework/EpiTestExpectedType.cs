using System;
namespace epi.TestFramework
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EpiTestExpectedType : Attribute{}
}
