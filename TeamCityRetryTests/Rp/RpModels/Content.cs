using System.Runtime.Serialization;
using ReportPortal.Client.Models;

namespace TeamCityRetryTests.Rp.RpModels
{
    [DataContract]
    public class Content:Launch
    {
        [DataMember(Name = "status")]
        public Status Status { get; set; }

    }
}