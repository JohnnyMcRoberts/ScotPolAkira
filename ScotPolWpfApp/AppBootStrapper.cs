

using System;
using System.IO;
using System.Linq;
using System.Windows;
using ElectionDataTypes.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScotPolWpfApp.Models;

namespace ScotPolWpfApp
{
    using Caliburn.PresentationFramework.ApplicationModel;

    using ScotPolWpfApp.ViewModels;

    public class AppBootStrapper : Bootstrapper<MainViewModel>
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            if (Configuration is ConfigurationRoot configurationRoot)
            {
                var providers = configurationRoot.Providers;
                string value = null;
                providers.FirstOrDefault()?.TryGet("DatabaseSettings:DatabaseConnectionString", out value);
                ConfigurationSettings.DatabaseSettings.DatabaseConnectionString = value ?? string.Empty;

                providers = configurationRoot.Providers;
                value = null;
                providers.FirstOrDefault()?.TryGet("DatabaseSettings:ExportDirectory", out value);
                ConfigurationSettings.DatabaseSettings.ExportDirectory = value ?? string.Empty;

                providers = configurationRoot.Providers;
                value = null;
                providers.FirstOrDefault()?.TryGet("DatabaseSettings:PredictionsDirectory", out value);
                ConfigurationSettings.DatabaseSettings.PredictionsDirectory = value ?? string.Empty;

                providers = configurationRoot.Providers;
                value = null;
                providers.FirstOrDefault()?.TryGet("DatabaseSettings:ResultsDirectory", out value);
                ConfigurationSettings.DatabaseSettings.ResultsDirectory = value ?? string.Empty;

                //onfigurationRoot["DatabaseSettings:DatabaseConnectionString"];
                //ConfigurationSettings.DatabaseSettings.ExportDirectory =
                //    configurationRoot["DatabaseSettings:ExportDirectory"];
                //ConfigurationSettings.DatabaseSettings.PredictionsDirectory =
                //    configurationRoot["DatabaseSettings:PredictionsDirectory"];
                //ConfigurationSettings.DatabaseSettings.ResultsDirectory =
                //    configurationRoot["DatabaseSettings:ResultsDirectory"];
            }

            ConfigurationSettings.DatabaseSettings.DatabaseConnectionString = "";

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(sender, e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ...

            services.AddTransient(typeof(DatabaseSettings));
        }
    }
}
