using System.Collections.Generic;
using CommandLine;

namespace TeamCityRetryTests
{
    public class Options
    {
        [Option('h', "hostName", Required = true, HelpText = "Set teamcity hostName")]
        public string HostName { get; set; }

        [Option('c', "credentials", Max = 2, Min = 2, Required = true, Separator = ';',
            HelpText = "Set teamcity Credentials")]
        public IEnumerable<string> Credentials { get; set; }

        [Option('b', "BuildTypesId", Min = 1, Required = true, Separator = ';', HelpText = "Set buildId.")]
        public IEnumerable<string> BuildTypesId { get; set; }

        [Option('f', "filter", Required = true, HelpText = "Set filter.")]
        public string TestCaseProperty { get; set; }

        [Option('p', "props", Separator = ';', Required = false)]
        public IEnumerable<string> Properties { get; set; }
    }
}