using System;
using System.Collections.Generic;
using TeamCityRetryTests.TeamCity.Connection;
using TeamCityRetryTests.TeamCity.DomainEntities;
using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    public class Builds : IBuilds
    {
        #region Attributes

        private readonly ITeamCityCaller _mCaller;
        private string _mFields;

        #endregion

        #region Constructor

        internal Builds(ITeamCityCaller caller)
        {
            _mCaller = caller;
        }

        #endregion

        #region Public Methods

        public Builds GetFields(string fields)
        {
            var newInstance = (Builds) MemberwiseClone();
            newInstance._mFields = fields;
            return newInstance;
        }

        public List<Build> ByBuildLocator(BuildLocator locator)
        {
            var buildWrapper =
                _mCaller.GetFormat<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator={0}", _mFields), locator);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> ByBuildLocator(BuildLocator locator, List<String> param)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                _mCaller.Get<BuildWrapper>(
                    ActionHelper.CreateFieldUrl($"/builds?locator={locator}{strParam}", _mFields));

            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public Build ById(string id)
        {
            var build = _mCaller.GetFormat<Build>(ActionHelper.CreateFieldUrl("/builds/id:{0}", _mFields), id);

            return build ?? new Build();
        }

        public List<Build> AllSinceDate(DateTime date, long count = 100, List<string> param = null)
        {
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            param.Add($"count({count})");

            return ByBuildLocator(BuildLocator.WithDimensions(sinceDate: date), param);
        }

        public List<Build> AllRunningBuild()
        {
            var buildWrapper =
                _mCaller.GetFormat<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator=running:true", _mFields));
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(sinceDate: date, status: buildStatus));
        }

        public List<Build> RetrieveEntireBuildChainFrom(string buildId, bool includeInitial = true,
            List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            return GetBuildListQuery(
                "/builds?locator=snapshotDependency:(from:(id:{0}),includeInitial:" + strIncludeInitial + "){1}",
                buildId, param);
        }

        public List<Build> RetrieveEntireBuildChainTo(string buildId, bool includeInitial = true,
            List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            return GetBuildListQuery(
                "/builds?locator=snapshotDependency:(to:(id:{0}),includeInitial:" + strIncludeInitial + "){1}", buildId,
                param);
        }


        public void DownloadLogs(string projectId, bool zipped, Action<string> downloadHandler)
        {
            var url = $"/downloadBuildLog.html?buildId={projectId}&archived={zipped}";
            _mCaller.GetDownloadFormat(downloadHandler, url, false);
        }

        #endregion

        #region Private Methods

        private static string GetParamLocator(List<string> param)
        {
            var strParam = "";
            if (param != null)
            {
                foreach (var tmpParam in param)
                {
                    strParam += ",";
                    strParam += tmpParam;
                }
            }

            return strParam;
        }

        private List<Build> GetBuildListQuery(string url, string id, List<string> param = null)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                _mCaller.GetFormat<BuildWrapper>(
                    ActionHelper.CreateFieldUrl(
                        url, _mFields),
                    id, strParam);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        #endregion
    }
}
