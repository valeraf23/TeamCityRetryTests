using System.Collections.Generic;
using System.Runtime.Serialization;
using ReportPortal.Client.Models;

namespace TeamCityRetryTests.Rp.RpModels
{
    [DataContract]
    public class LaunchesContents
    {
        [DataMember(Name = "content")]
        public List<Content> Launches { get; set; }
        [DataMember(Name = "page")]
        public Page Page { get; set; }
    }
}