using System.Collections.Generic;
using TeamCityRetryTests.TeamCity.DomainEntities;
using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    public interface IBuildQueue
    {
        List<Build> All();

        BuildQueue GetFields(string fields);
        List<Build> ByBuildTypeLocator(BuildTypeLocator locator);
        Build ById(string id);
        List<Build> ByProjectLocater(ProjectLocator projectLocator);
        Build RunBuild(string rerunBuildId, IDictionary<string,string> properties);
    }
}