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

        public string PluginVersion => "1.0.0";

        public string PluginAuthor => "Class1";

        public string PluginWebsite => "21321";

        public bool IsSupportSettings => false;

        public void Settings()
        {
            throw new NotImplementedException();
        }
    }
}
