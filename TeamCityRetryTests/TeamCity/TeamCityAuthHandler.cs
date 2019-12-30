using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRetryTests.TeamCity
{
    public class TeamCityAuthHandler : DelegatingHandler
    {
        private readonly string _userName;
        private readonly string _password;

        public TeamCityAuthHandler(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = Encoding.ASCII.GetBytes($"{_userName}:{_password}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            return base.SendAsync(request, cancellationToken);
        }
    }
}