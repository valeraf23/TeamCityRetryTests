using System.Collections.Generic;

namespace TeamCityRetryTests.Rp
{
    internal static class GetLaunchName
    {
        private static readonly Dictionary<string, string> Dict = new Dictionary<string, string>
        {
            ["IpreoPcsApiTests_AdminApiTests_AdminBeta"] = "Admin Beta"
        };

        public static string Get(string buildType) => Dict[buildType];
    }
}