namespace TeamCityRetryTests.Rp
{
    public class ReportPortalOptional
    {
        public string Url { get; set; }
        public string Project { get; set; }
        public Authentication Authentication { get; set; }
       
    }

    public class Authentication
    {
        public string Uuid { get; set; }
    }
}