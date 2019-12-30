using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class VcsRootEntry
  {
    public override string ToString()
    {
      return "vcs-root-entry";
    }

    [JsonProperty("vcs-root")]
    public VcsRoot VcsRoot { get; set; }

    [JsonProperty("checkout-rules")]
    public string CheckoutRules { get; set; }
  }
}