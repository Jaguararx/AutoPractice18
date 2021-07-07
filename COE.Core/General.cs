using Atata;
using COE.Core.Report.ZephyrAPI;
using System;
using Atata.Configuration.Json;

namespace COE.Core
{
    public static class General
    {
        public static AppData AppData;
        public static ZAPIManager ZAPIManager;
        public static bool ConfigApplied = false;
        public static Exception SetUpFixtureException = null;

        static General()
        {
        }

        /// <summary>
        ///   Build Atata Context method
        /// </summary>
        public static AtataContextBuilder PreparedContext()
        {
            return AtataContext.Configure()
                .UseNUnitTestName()
                .AddScreenshotFileSaving()
                .WithFolderPath(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\{build-start}\{test-name}");
        }

    }
}