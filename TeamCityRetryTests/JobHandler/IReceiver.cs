using System.Collections.Generic;

namespace TeamCityRetryTests.JobHandler
{
    public interface IReceiver<in T>
    {
        void Handle(IEnumerable<T> request);
    }
}