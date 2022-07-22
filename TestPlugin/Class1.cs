using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinVPN.Plugin.SDK;

namespace TestPlugin
{
    public class Class1 : WinVPNPlugin, IPlugin
    {
        public string PluginName => "TestPlugin";

        public Version PluginVersion => new Version("1.0.0");

        public string PluginAuthor => "Class1";

        public string PluginWebsite => "21321";

        public bool IsSupportSettings => false;

        public Version MiniDependentVersion => new Version("1.2.0");

        public void Settings()
        {
            throw new NotImplementedException();
        }
    }
}
