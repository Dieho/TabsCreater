using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Hellpers
{
    public class ConfigBuilder
    {
        private readonly Config _config = new Config();
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
            var name = "\\config.xml";
            try
            {
                return XDocument.Load(configsDirectory + name);
            }
            catch (Exception)
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
