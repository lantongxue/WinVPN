using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using WinVPN.Plugin;

namespace WinVPN.Service
{
    internal class PluginService
    {
        readonly string PluginPath = "plugin";

        List<TabItem> _tabitems = new List<TabItem>();

        List<object> _plugins = new List<object>();

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

                Assembly plugin = Assembly.LoadFile(file.FullName);
                foreach(Type exp in plugin.ExportedTypes)
                {
                    if(exp.BaseType.FullName == "WinVPN.Plugin.Plugin" && exp.GetInterface("WinVPN.Plugin.IPlugin") != null)
                    {
                        object obj = Activator.CreateInstance(exp);
                        _plugins.Add(obj);

                        MethodInfo GetTabItems = exp.GetMethod("GetTabItems");
                        if(GetTabItems != null)
                        {
                            IEnumerable<TabItem> items = (TabItem[])GetTabItems.Invoke(obj, null);
                            _tabitems.AddRange(items);
                        }
                        break;
                    }
                }
            }
        }

        public List<TabItem> GetTabItems()
        {
            return _tabitems;
        }

        public List<object> GetPlugins()
        {
            return _plugins;
        }
    }
}
