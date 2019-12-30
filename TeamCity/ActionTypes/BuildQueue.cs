using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamCityRetryTests.TeamCity.Connection;
using TeamCityRetryTests.TeamCity.DomainEntities;
using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    public class BuildQueue : IBuildQueue
    {
        private readonly ITeamCityCaller _mCaller;
        private string _mFields;

        internal BuildQueue(ITeamCityCaller caller)
        {
            _mCaller = caller;
        }

        public BuildQueue GetFields(string fields)
        {
            var newInstance = (BuildQueue) MemberwiseClone();
            newInstance._mFields = fields;
            return newInstance;
        }

        public List<Build> All()
        {
            var buildQueue =
                _mCaller.Get<BuildWrapper>(ActionHelper.CreateFieldUrl("/buildQueue", _mFields));

            return buildQueue.Build;
        }

        public List<Build> ByBuildTypeLocator(BuildTypeLocator locator)
        {
            var buildWrapper = _mCaller.Get<BuildWrapper>($"/buildQueue?locator=buildType:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> ByProjectLocater(ProjectLocator locator)
        {
            var buildWrapper = _mCaller.Get<BuildWrapper>($"/buildQueue?locator=project:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }
        public Build ById(string id) => _mCaller.Get<Build>($"/buildQueue/{id}");

        public Build RunBuild(string rerunBuildId, IDictionary<string, string> properties)
        {
            var p = properties.Aggregate(new StringBuilder(),
                (s, v) => s.AppendLine($"<property name='{v.Key}' value='{v.Value}'/>"), s => s.ToString());
            var xmlData =
                "<build>" +
                $"<buildType id='{rerunBuildId}'/>" +
                "<properties>" +
                $"{p}" +
                "</properties>" +
                "</build>";
            return _mCaller.Post<Build>(xmlData, HttpContentTypes.ApplicationXml, "/buildQueue",
                HttpContentTypes.ApplicationJson);
        }
    }
}