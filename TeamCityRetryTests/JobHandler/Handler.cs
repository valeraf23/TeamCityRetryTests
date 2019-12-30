using System.Collections.Generic;
using System.Linq;
using TeamCityRetryTests.Jobs;

namespace TeamCityRetryTests.JobHandler
{
    public class Handler
    {
        private readonly Queue<IReceiver<Job>> _receivers;

        public Handler(IReceiver<Job> receiver, params IReceiver<Job>[] receivers)
        {

            var r = new List<IReceiver<Job>> {receiver};
            if (receivers != null && receivers.Any())
            {
                r.AddRange(receivers);

            }
            _receivers = new Queue<IReceiver<Job>>(r);
        }
        public void Handle(IList<Job> jobs)
        {
            foreach (var receiver in _receivers)
            {
                receiver.Handle(jobs);
            }
        }
    }
}