using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class ParentProjectWrapper
  {
    private readonly ProjectLocator _locator;

    public ParentProjectWrapper(ProjectLocator locator)
    {
      _locator = locator;
    }

    public string Locator
    {
      get { return _locator.ToString(); }
    }
  }
}