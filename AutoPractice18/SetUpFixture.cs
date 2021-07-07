using Atata;
using NUnit.Framework;
using COE.Core;
using Atata.Configuration.Json;
using Newtonsoft.Json;
using System;


/// <summary>
/// Examples of Testing automation package
/// </summary>
namespace AutoPractice18
{
    /// <summary>
    /// Class for setting up nunit test fixture
    /// </summary>
    class SetUpFixture : SetUpFixtureBase
    {
        public SetUpFixture()
        {
            string key = "download.default_directory";
            string val = "c:\\temp2";
            string jsonContent = JsonConfigFile.ReadText(null, null);
            AppConfig config = JsonConvert.DeserializeObject<AppConfig>(jsonContent);
            foreach (DriverJsonSection drivers in config.Drivers) {
                if (drivers.Type == "chrome") {
                    if (drivers.Options.UserProfilePreferences == null)
                    {
                        drivers.Options.UserProfilePreferences = new JsonSection();
                        drivers.Options.UserProfilePreferences.AdditionalProperties.Add(key, val);
                    }
                    else {
                        if (drivers.Options.UserProfilePreferences.AdditionalProperties.ContainsKey(key))
                        {
                            drivers.Options.UserProfilePreferences.AdditionalProperties[key] = val;
                        }
                        else {
                            drivers.Options.UserProfilePreferences.AdditionalProperties.Add(key, val);
                        }
                    }
                }
            }
            ApplyGlobalConfig(config, jsonContent);
        }

        /// <summary>
        ///   One Time Setup method for Test Fixture. 
        /// </summary>
        [OneTimeSetUp]
        public void UITestsSetUp()
        {
            try
            {
                if (General.AppData.OneTimeDriverSetup && General.AppData.CustomBuildDriver)
                {
                    General.PreparedContext().Build();                    
                }

            }
            catch (Exception e)
            {
                General.SetUpFixtureException = e;
            }
        }
        /// <summary>
        ///   One Time Tear down method for Test Fixture. Clean up current Atata Context 
        /// </summary>
        [OneTimeTearDown]
        public void UITestsTearDown()
        {
            if (General.AppData.OneTimeDriverSetup)
            {
                AtataContext.Current?.CleanUp();
            }
        }
    }
}
