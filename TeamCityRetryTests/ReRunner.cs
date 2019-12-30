using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartWait;
using TeamCityRetryTests.Helpers;
using TeamCityRetryTests.Jobs;
using TeamCityRetryTests.Rp;
using TeamCityRetryTests.TeamCity;
using TeamCityRetryTests.TeamCity.DomainEntities;
using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests
{
    public class ReRunner
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly ITeamCityClient _client;
        private readonly RpClient _rpClient;
        private readonly ConcurrentQueue<Worker> _queue = new ConcurrentQueue<Worker>();
        private readonly ILogger _logger;
        private string TestCaseFilter { get; set; } = "TestCaseFilter";

        public ReRunner(ITeamCityClient client, RpClient rpClient, ILoggerFactory logger)
        {
            _client = client;
            _rpClient = rpClient;
            _logger = logger.CreateLogger<ReRunner>();
        }

        public ReRunner AddProperties(Dictionary<string, string> props)
        {
            if (props == null || !props.Any())
            {
                return this;
            }

            _properties.AddRange(props);
            return this;
        }

        public ReRunner AddTestCaseFilter(string testCaseFilter)
        {
            TestCaseFilter = testCaseFilter;
            return this;
        }

        public ReRunner For(IEnumerable<string> optionsBuildTypesId)
        {
            GetFilterQuery(optionsBuildTypesId);
            return this;
        }

        public void Run()
        {
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(_queue, worker =>
            {
                try
                {
                    worker.Perform();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    exceptions.Enqueue(e);
                }
            });
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

        private IEnumerable<Job> ReRun(string buildTypeId, IDictionary<string, string> props) => new Job[]
            {GetTeamCityJob(buildTypeId, props), GetReportPortalJob(buildTypeId)};

        private static IEnumerable<(string Id, string BuildTypeId)> GetBuildsId(ITeamCityClient client,
            IEnumerable<string> buildTypesId) =>
            client.Builds.AllBuildsOfStatusSinceDate(DateTime.Today, BuildStatus.FAILURE)
                .Where(x => buildTypesId.Contains(x.BuildTypeId)).GroupBy(a => a.BuildTypeId, c => c.Id)
                .Select(f => (f.Max(), f.Key));

        private void GetFilterQuery(IEnumerable<string> buildInfos)
        {
            foreach (var buildInfo in GetBuildsId(_client, buildInfos))
            {
                var (id, buildTypeId) = buildInfo;
                var job = CallReRunBuild(id, buildTypeId);
                _queue.Enqueue(job);
            }
        }

        private Worker CallReRunBuild(string id, string buildTypeId)
        {
            var props = GetParametersForBuildConfigurations(id);
            var jobs = props.Any()
                ? ReRun(buildTypeId, props)
                : new[] {new DefaultJob(() => _logger.LogInformation("There are no failed tests"))};
            return new Worker(jobs);
        }

        private TeamCityJob GetTeamCityJob(string buildTypeId, IDictionary<string, string> props)
        {
            return new TeamCityJob(() =>
            {
                _logger.LogInformation($"Run: {buildTypeId}");
                var build = _client.BuildQueue.RunBuild(buildTypeId, props);
                WaitFor.Condition(() => _client.BuildQueue.ById(build.Id).State == "finished", builder => builder
                        .SetMaxWaitTime(TimeSpan.FromMinutes(30))
                        .SetTimeBetweenStep(retryAttempt =>
                            TimeSpan.FromMinutes(Math.Pow(4, 1 / (double) retryAttempt) + 2))
                        .Build(),
                    $"Job: \"{build.Id}\" still in progress");
            });
        }

        private ReportPortalJob GetReportPortalJob(string buildTypeId)
        {
            return new ReportPortalJob(() =>
                {
                    _logger.LogInformation($"Start Merge main and rerun launches for {GetLaunchName.Get(buildTypeId)}");
                    _rpClient.MergeTwoLaunches(GetLaunchName.Get(buildTypeId)).GetAwaiter().GetResult();
                }
            );
        }

        private Dictionary<string, string> GetParametersForBuildConfigurations(string id)
        {
            var projects = _client.Tests.FailedTestsByBuildId(id);

            if (projects == null)
            {
                return new Dictionary<string, string>();
            }

            var query = GetFailedTestsFilter(projects);
            return new Dictionary<string, string>(_properties) {{TestCaseFilter, query}};
        }

        private static string GetFailedTestsFilter(IEnumerable<TestOccurrence> failedTests)
        {
            var f = failedTests.Select(t => $"FullyQualifiedName~{GetFullyQualifiedNameForTestClass(t.Name)}")
                .Distinct();
            return string.Join("|", f);
        }

        private static string GetFullyQualifiedNameForTestClass(string failedTestName)
        {
            const string excludeText = "TRX: Ipreo.PCS.Api.Tests.";
            var query = failedTestName.Split(new[] {excludeText}, StringSplitOptions.None).Last().Split('.')
                .SkipLast(1);
            return string.Join('.', query);
        }
    }
}