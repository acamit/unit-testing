using System.Configuration;

namespace MediaStorage.Config
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string CanGetAllMenus()
        {
            return ConfigurationManager.AppSettings["CanGetAllMenus"];
        }
    }
}
