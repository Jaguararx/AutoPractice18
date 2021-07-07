using Allure.Commons;
using Atata;
using Atata.Configuration.Json;
using COE.Core.Report.ZephyrAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

/// <summary>
/// Core of Testing automation package 
/// </summary>
namespace COE.Core
{
    /// <summary>
    /// Base Class for setting up nunit test fixture
    /// </summary>
    [SetUpFixture]
    public class SetUpFixtureBase
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        private static PropertyInfo GetConfigProperty<TConfig>(string name)
        {
            Type type = typeof(TConfig);
            PropertyInfo property = type.GetProperty(name, BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return property ?? throw new MissingMemberException(type.FullName, name);
        }

        public void ApplyGlobalConfig<TConfig>(TConfig config, String jsonContent)
             where TConfig : JsonConfig<TConfig>, new()
        {
            PropertyInfo globalConfigProperty = GetConfigProperty<TConfig>(nameof(JsonConfig.Global));
            if (globalConfigProperty.GetValue(null, null) is TConfig currentConfig)
                JsonConvert.PopulateObject(jsonContent, currentConfig);
            else
                globalConfigProperty.SetValue(null, config, null);
            PropertyInfo currentConfigProperty = GetConfigProperty<TConfig>(nameof(JsonConfig.Current));

            if (currentConfigProperty.GetValue(null, null) == null)
            {
                object globalValue = GetConfigProperty<TConfig>(nameof(JsonConfig.Global)).GetValue(null, null);

                if (globalValue != null)
                {
                    string serializedGlobalValue = JsonConvert.SerializeObject(globalValue, SerializerSettings);

                    object clonedGlobalValue = JsonConvert.DeserializeObject(serializedGlobalValue, globalValue.GetType());
                    currentConfigProperty.SetValue(null, clonedGlobalValue, null);
                }
            }
            AtataContext.GlobalConfiguration.ApplyJsonConfig(config);
        }

        /// <summary>
        ///   One Time Setup method for Test Fixture. Creates Atata Context builder, configure it and build
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            try
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
                AllureExtensions.WrapSetUpTearDownParams(() => { AllureLifecycle.Instance.CleanupResultDirectory(); });
                General.AppData = new AppData();
                if (General.AppData.ZephyrEnabled)
                {
                    General.ZAPIManager = new ZAPIManager();
                }
                if (AtataContext.GlobalConfiguration.BuildingContext.DriverFactoryToUse == null)
                {
                    AtataContext.GlobalConfiguration.ApplyJsonConfig<AppData>();
                }
                if (General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver)
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
        public void TearDown()
        {
            if (General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver)
            {
                AtataContext.Current?.CleanUp();
            }
        }

    }
}
