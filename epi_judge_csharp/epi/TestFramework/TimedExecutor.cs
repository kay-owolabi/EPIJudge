using System;
namespace epi.TestFramework
{
    public class TimedExecutor<ReturnType>
    {
        public TimedExecutor(long timeoutSeconds)
        {
        }

        public ReturnType Run(Fu<ReturnType> func)
        {
            throw new NotImplementedException();
        }
    }
}
