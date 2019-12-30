using System;
using System.Net.Http;

namespace TeamCityRetryTests.TeamCity.Connection
{
    public interface ITeamCityCaller
    {
   
        T GetFormat<T>(string urlPart, params object[] parts);

        void GetFormat(string urlPart, params object[] parts);

        T PostFormat<T>(object data, string contenttype, string accept, string urlPart, params object[] parts);

        void PostFormat(object data, string contenttype, string urlPart, params object[] parts);

        T PutFormat<T>(object data, string contenttype, string accept, string urlPart, params object[] parts);

        void PutFormat(object data, string contenttype, string urlPart, params object[] parts);

        void DeleteFormat(string urlPart, params object[] parts);

        void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts);

        void GetDownloadFormat(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts);

        T Get<T>(string urlPart);

        void Get(string urlPart);

        T Post<T>(object data, string contenttype, string urlPart, string accept);

        T Put<T>(object data, string contenttype, string urlPart, string accept);
        
        HttpResponseMessage Post(object data, string contenttype, string urlPart, string accept);

        HttpResponseMessage Put(object data, string contenttype, string urlPart, string accept);

        void Delete(string urlPart);

        string GetRaw(string urlPart);

        string GetRaw(string urlPart, bool rest);
    }
}