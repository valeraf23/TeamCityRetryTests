namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    public class ActionHelper
    {

        public static string CreateFieldUrl(string url, string fields)
        {
            if (string.IsNullOrEmpty(fields)) return url;
            if (url.Contains("?"))
                url += "&fields=" + fields;
            else
                url += "?fields=" + fields;
            return url;
        }
    }
}