using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using TeamCityRetryTests.Helpers;
using File = System.IO.File;

namespace TeamCityRetryTests.TeamCity.Connection
{
    internal class TeamCityCaller : ITeamCityCaller
    {
        private readonly HttpClient _httpClient;

        public TeamCityCaller(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient(HttpClientName.TeamCity);
        }

        public T GetFormat<T>(string urlPart, params object[] parts)
        {
            return Get<T>(string.Format(urlPart, parts));
        }

        public void GetFormat(string urlPart, params object[] parts)
        {
            Get(string.Format(urlPart, parts));
        }

        public T PostFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts)
        {
            return Post<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public void PostFormat(object data, string contentType, string urlPart, params object[] parts)
        {
            Post(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public T PutFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts)
        {
            return Put<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public void PutFormat(object data, string contentType, string urlPart, params object[] parts)
        {
            Put(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public void DeleteFormat(string urlPart, params object[] parts)
        {
            Delete(string.Format(urlPart, parts));
        }

        public void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts)
        {
            GetDownloadFormat(downloadHandler, urlPart, true, parts);
        }

        public void GetDownloadFormat(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts)
        {
            if (downloadHandler == null)
                throw new ArgumentException("A download handler must be specified.");

            var tempFileName = Path.GetRandomFileName();
            var url = rest ? CreateUrl(string.Format(urlPart, parts)) : CreateUrl(string.Format(urlPart, parts), false);

            try
            {
                AddAccept(HttpContentTypes.ApplicationJson)
                    .GetAsFile(url, tempFileName);
                downloadHandler.Invoke(tempFileName);
            }
            finally
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
            }
        }

        public T Get<T>(string urlPart)
        {
            var response = GetResponse(urlPart);
            return response.StaticBody<T>();
        }

        public void Get(string urlPart)
        {
            GetResponse(urlPart);
        }

        private HttpResponseMessage GetResponse(string urlPart)
        {
            var url = CreateUrl(urlPart);

            var response =
                AddAccept(HttpContentTypes.ApplicationJson)
                    .Get(url);
            ThrowIfHttpError(response, url);
            return response;
        }

        public T Post<T>(object data, string contentType, string urlPart, string accept)
        {
            return Post(data, contentType, urlPart, accept).StaticBody<T>();
        }

        public T Put<T>(object data, string contentType, string urlPart, string accept)
        {
            return Put(data, contentType, urlPart, accept).StaticBody<T>();
        }

        public HttpResponseMessage Post(object data, string contentType, string urlPart, string accept)
        {
            var response = MakePostRequest(data, contentType, urlPart, accept);

            return response;
        }

        public HttpResponseMessage Put(object data, string contentType, string urlPart, string accept)
        {
            var response = MakePutRequest(data, contentType, urlPart, accept);

            return response;
        }

        public void Delete(string urlPart) => MakeDeleteRequest(urlPart);

        private void MakeDeleteRequest(string urlPart)
        {
            var client = AddAccept(HttpContentTypes.TextPlain);
            var url = CreateUrl(urlPart);
            var response = client.Delete(url);
            ThrowIfHttpError(response, url);
        }

        private HttpResponseMessage MakePostRequest(object data, string contentType, string urlPart, string accept)
        {
            var client = AddAccept(string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept);

            var url = CreateUrl(urlPart);
            var response = client.Post(url, data, contentType);
            ThrowIfHttpError(response, url);

            return response;
        }

        private HttpResponseMessage MakePutRequest(object data, string contentType, string urlPart, string accept)
        {
            var client = AddAccept(
                string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept);
            var url = CreateUrl(urlPart);
            var response = client.Put(url, data, contentType);
            ThrowIfHttpError(response, url);

            return response;
        }

        private static bool IsHttpError(HttpResponseMessage response)
        {
            var num = (int) response.StatusCode / 100;

            return (num == 4 || num == 5);
        }

        private static void ThrowIfHttpError(HttpResponseMessage response, string url)
        {
            if (!IsHttpError(response))
                return;
            throw new HttpException(response.StatusCode,
                $"Error: {response.ReasonPhrase}\nHTTP: {response.StatusCode}\nURL: {url}\n{response.RawText()}");
        }

        private static string CreateUrl(string urlPart, bool rest = true)
        {
            var restUrl = rest ? "/app/rest" : "";
            var uri = $"{restUrl}{urlPart}";
            return Uri.EscapeUriString(uri).Replace("+", "%2B");
        }

        private HttpClient AddAccept(string accept)
        {
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(accept));
            return _httpClient;
        }

        public string GetRaw(string urlPart)
        {
            return GetRaw(urlPart, true);
        }

        public string GetRaw(string urlPart, bool rest)
        {
            var url = rest ? CreateUrl(urlPart) : CreateUrl(urlPart, false);

            var httpClient = AddAccept(HttpContentTypes.TextPlain);
            var response = httpClient.Get(url);
            if (IsHttpError(response))
            {
                throw new HttpException(response.StatusCode,
                    $"Error {response.ReasonPhrase}: Thrown with URL {url}");
            }

            return response.RawText();
        }

        private static string GetContentType(string data) => data.StartsWith("<") ? HttpContentTypes.ApplicationXml : HttpContentTypes.TextPlain;
    }
}