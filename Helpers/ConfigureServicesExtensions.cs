using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TeamCityRetryTests.Policy;
using TeamCityRetryTests.Rp;
using TeamCityRetryTests.TeamCity;
using TeamCityRetryTests.TeamCity.Connection;

namespace TeamCityRetryTests.Helpers
{
    public static class ConfigureServicesExtensions
    {
        private const string PoliciesConfigurationSectionName = "Policies";
        private const string ReportPortalConfigurationSectionName = "server";

        public static IServiceCollection ConfigureServicesForTeamCity(this IServiceCollection services,
            Options args)
        {
            var creds = args.Credentials.ToArray();
            services.AddHttpClient(HttpClientName.TeamCity, client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = new Uri($"{args.HostName}httpAuth");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Origin", args.HostName);
                }).AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
               .AddHttpMessageHandler(p => new TeamCityAuthHandler(creds[0], creds[1]));

            services.AddScoped<ITeamCityCaller, TeamCityCaller>();
            services.AddScoped<ITeamCityClient, TeamCityClient>();
            return services;
        }

        public static IServiceCollection ConfigureServicesForReportPortal(this IServiceCollection services, IConfiguration configuration,
            string configurationSectionName = ReportPortalConfigurationSectionName)
        {
            var section = configuration.GetSection(configurationSectionName);
            services.Configure<ReportPortalOptional>(section);
            var reportPortalOptional = section.Get<ReportPortalOptional>();

            services.AddHttpClient(HttpClientName.ReportPortal, client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = new Uri(reportPortalOptional.Url);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + reportPortalOptional.Authentication.Uuid);
                }).AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker);
            services.AddScoped<RpClient>();
            return services;
        }

        public static IServiceCollection AddPolicies(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = PoliciesConfigurationSectionName)
        {
            var section = configuration.GetSection(configurationSectionName);
            services.Configure<PolicyOptions>(configuration);
            var policyOptions = section.Get<PolicyOptions>();

            var policyRegistry = services.AddPolicyRegistry();
            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        policyOptions.HttpRetry.Count,
                        retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(policyOptions.HttpRetry.Power, retryAttempt))));
            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: policyOptions.HttpCircuitBreaker
                            .ExceptionsAllowedBeforeBreaking,
                        durationOfBreak: policyOptions.HttpCircuitBreaker.DurationOfBreak));

            return services;
        }
    }
}