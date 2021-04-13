using System;
using System.Collections.Generic;
using System.Text;
using ElectionDataTypes.Settings;

namespace ScotPolWpfApp.Models
{
    public static class ConfigurationSettings
    {
        public static DatabaseSettings DatabaseSettings { get; set; }

        static ConfigurationSettings()
        {
            DatabaseSettings = new DatabaseSettings();
        }
    }
}
