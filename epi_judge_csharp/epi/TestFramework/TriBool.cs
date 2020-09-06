using System;
namespace epi.TestFramework
{
    public enum TriBool
    {
        FALSE,
        TRUE,
        INDETERMINATE,
    }

    public static class Extension
    {
        public static bool GetOrDefault(this TriBool @bool, bool defaultValue)
        {
            return @bool switch
            {
                TriBool.FALSE => false,
                TriBool.TRUE => true,
                _ => defaultValue,
            };
        }
    }
}
