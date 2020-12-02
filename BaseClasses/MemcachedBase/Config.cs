using Enyim.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses.MemcachedBase
{
    internal class Config
    {
        static System.Configuration.Configuration DefaultConfiguration
        {
            get
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/MemCached.config");
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = path;

                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                return config;
            }
        }

        static MemcachedClientSection configureSection;
        public static MemcachedClientSection Configurations
        {
            get
            {
                if (configureSection == null)
                {
                    configureSection = DefaultConfiguration.GetSection("memcached") as MemcachedClientSection;
                }
                if (configureSection == null)
                {
                    throw new SettingsPropertyNotFoundException("memcached配置表配置出错");
                }
                return configureSection;
            }
        }
    }
}
