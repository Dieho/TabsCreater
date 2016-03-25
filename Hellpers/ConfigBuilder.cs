using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Hellpers
{
    public class ConfigBuilder
    {
        private Config _config = new Config();
        private XDocument _settings;

        public ConfigBuilder New()
        {
            Configure(Environment.CurrentDirectory);
            return this;
        }

        private void Configure(string configsDirectoryPath)
        {
            _settings = ReadSettings(configsDirectoryPath);

            _config.Name = GetSettingFromXml<string>("//Name");
            _config.SoundPressure = GetSettingFromXml<double>("//SoundPressureBarriere");
        }

        private T GetSettingFromXml<T>(string settingName)
        {
            var element = _settings.XPathSelectElement(settingName);
            return (T)Convert.ChangeType(element.Value, typeof(T));
        }

        private XDocument ReadSettings(string configsDirectoryPath)
        {
            var configsDirectory = new DirectoryInfo(configsDirectoryPath);
            if (!DirectoryExists(configsDirectory))
            {
                throw new Exception("Configs directory was not found");
            }
            var name = "\\standart.xml";
            try
            {
                return XDocument.Load(configsDirectory + name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool DirectoryExists(DirectoryInfo dir)
        {
            return dir != null && dir.Exists;
        }
        public Config Build()
        {
            return _config;
        }
    }
}
