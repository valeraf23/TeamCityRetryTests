using System;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamCityRetryTests.Helpers;

namespace TeamCityRetryTests
{
    class Program
    {
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("ReportPortal.config.json", true);

            _configuration = builder.Build();
            var serviceCollection = new ServiceCollection();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(y => y.Credentials = y.Credentials.Select(b => b.Trim()))
                .WithParsed(y => y.Properties = y.Properties.Select(b => b.Trim()))
                .WithParsed(y => y.HostName = y.HostName.Trim())
                .WithParsed(y => y.BuildTypesId = y.BuildTypesId.Select(b => b.Trim()))
                .WithParsed(y => y.TestCaseProperty = y.TestCaseProperty.Trim())
                .WithParsed(x => ConfigureServices(serviceCollection, x));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<IProgramService>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, Options args)
        {
            serviceCollection.AddLogging(configure => configure.AddConsole());
            serviceCollection.AddScoped(p => args);
            serviceCollection
                .AddPolicies(_configuration)
                .ConfigureServicesForTeamCity(args)
                .ConfigureServicesForReportPortal(_configuration);
            serviceCollection.AddScoped<ReRunner>();
            serviceCollection.AddScoped<IProgramService, ProgramService>();
        }
    }
}
