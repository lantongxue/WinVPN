using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using WinVPN.Plugin;
using System.Collections;
using WinVPN.Plugin.SDK;

namespace WinVPN.Service
{
    internal class PluginService
    {
        readonly string PluginPath = "plugin";

        List<TabItem> _tabitems = new List<TabItem>();

        Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

        public PluginService()
        {
            this.CheckPluginFolder();

            this.LoadPlugins();
        }

        protected void CheckPluginFolder()
        {
            if(!Directory.Exists(PluginPath))
            {
                Directory.CreateDirectory(PluginPath);
            }
        }

        public void LoadPlugins()
        {
            DirectoryInfo directory = new DirectoryInfo(PluginPath);
            foreach(FileInfo file in directory.GetFiles())
            {
                if(file.Extension.ToLower() != ".dll")
                {
                    continue;
                }
                this.LoadPlugin(file.FullName);
            }
        }

        public void LoadPlugin(string pluginPath)
        {
            Assembly plugin = Assembly.LoadFile(pluginPath);
            foreach (Type exp in plugin.ExportedTypes)
            {
                if (exp.BaseType == typeof(WinVPNPlugin) && exp.GetInterface("WinVPN.Plugin.SDK.IPlugin") != null)
                {
                    WinVPNPlugin obj = (WinVPNPlugin)plugin.CreateInstance(exp.FullName);
                    obj.IsEnable = true;
                    _plugins.Add(exp.FullName, (IPlugin)obj);
                    MethodInfo GetMainWindowTabItems = exp.GetMethod("GetMainWindowTabItems");
                    if (GetMainWindowTabItems != null)
                    {
                        IEnumerable<TabItem> items = (TabItem[])GetMainWindowTabItems.Invoke(obj, null);
                        if (items != null)
                        {
                            foreach (TabItem tab in items)
                            {
                                tab.Tag = exp.FullName;
                            }
                            _tabitems.AddRange(items);
                        }
                    }
                    break;
                }
            }
        }

        public List<TabItem> GetTabItems()
        {
            return _tabitems;
        }

        public Dictionary<string, IPlugin> GetPlugins()
        {
            return _plugins;
        }
    }
}
