using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReportPortal.Client;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using TeamCityRetryTests.Helpers;
using TeamCityRetryTests.Rp.RpModels;

namespace TeamCityRetryTests.Rp
{

    public class RpClient
    {
        public RpClient(IHttpClientFactory httpClient, IOptions<ReportPortalOptional> info, ILoggerFactory logger)
        {
            _client = httpClient.CreateClient(HttpClientName.ReportPortal);
            _info = info.Value;
            _logger = logger.CreateLogger<RpClient>();
            _service = new Service(new Uri(_info.Url), _info.Project, _info.Authentication.Uuid);
        }

        private readonly Service _service;
        private readonly HttpClient _client;
        private readonly ReportPortalOptional _info;
        private readonly ILogger _logger;

        public async Task MergeTwoLaunches(string launchName)
        {
            var mainLaunch = await GetLastLaunch(launchName);
            var rerunLaunch = await GetLastLaunch(launchName, true);
            var mainTestsIds = await GetFailedTestIds(mainLaunch.Id);

            await _service.MergeLaunchesAsync(new MergeLaunchesRequest
            {
                Description = mainLaunch.Description,
                Launches = new List<string>
                {
                    mainLaunch.Id, rerunLaunch.Id
                },
                MergeType = "DEEP",
                Name = launchName
            });

            var deleteTasks = mainTestsIds.Select(id => _service.DeleteTestItemAsync(id)).ToArray();
            var updateStatus = await UpdateStatus(rerunLaunch.Id);
            var task = deleteTasks.Concat(updateStatus);
            Task.WaitAll(task.Cast<Task>().ToArray());
            _logger.LogInformation($"Finish Merge main and rerun launches for {launchName}");
        }

        private async Task<Task<Message>[]> UpdateStatus(string launchId)
        {
            var tests = await GetTestIds(launchId);
            return tests.Select(x => _service.UpdateTestItemAsync(x, new UpdateTestItemRequest
            {
                Description = "Test was re-runed"
            })).ToArray();
        }

        private async Task<Content> GetLastLaunch(string launchName, bool isDebug = false)
        {
            return await GetLastLaunchesAsync(new FilterOption
            {
                Filters = new List<Filter> {new Filter(FilterOperation.Equals, "name", launchName)}
            }, isDebug).ContinueWith(l => l.Result.Launches[0]);
        }

        private async Task<string[]> GetFailedTestIds(string launchId)
        {
            var failedTestIds = await _service.GetTestItemsAsync(new FilterOption
            {
                Filters = new List<Filter> {new Filter(FilterOperation.Equals, "id", launchId)}
            }).ContinueWith(t =>
            {
                return t.Result.TestItems.Where(x => x.Status == Status.Failed).Select(x => x.Id).ToArray();
            });
            return failedTestIds;
        }

        private async Task<string[]> GetTestIds(string launchId)
        {
            var failedTestIds = await _service.GetTestItemsAsync(new FilterOption
            {
                Filters = new List<Filter> {new Filter(FilterOperation.Equals, "id", launchId)}
            }).ContinueWith(t => { return t.Result.TestItems.Select(x => x.Id).ToArray(); });
            return failedTestIds;
        }

        private async Task<LaunchesContents> GetLastLaunchesAsync(FilterOption filterOption = null, bool debug = false)
        {
            var uri = _service.BaseUri.Append($"{_info.Project}/launch/latest");
            if (debug)
            {
                uri = uri.Append("mode");
            }

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await _client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchesContents>(await response.Content.ReadAsStringAsync());
        }
    }
}