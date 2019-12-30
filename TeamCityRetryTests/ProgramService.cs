using TeamCityRetryTests.Helpers;

namespace TeamCityRetryTests
{
    public class ProgramService: IProgramService
    {
        private readonly ReRunner _runner;
        private readonly Options _options;

        public ProgramService(ReRunner runner, Options options)
        {
            _runner = runner;
            _options = options;
        }

        public void Run()
        {
            ReRun(_options);
        }
        private void ReRun(Options options)
        {
           _runner
                .AddTestCaseFilter(options.TestCaseProperty)
                .AddProperties(options.Properties.ToDictionary())
                .For(options.BuildTypesId)
                .Run();
        }
    }
}
