using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
    public class BuildTrigger
    {
        public BuildTrigger()
        {
            Properties = new Properties();
        }

        public override string ToString()
        {
            return "trigger";
        }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("disabled")] public bool Disabled { get; set; }

        [JsonProperty("inherited")] public bool Inherited { get; set; }

        [JsonProperty("properties")] public Properties Properties { get; set; }

    }
}