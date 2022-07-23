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
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WinVPN.Service
{
    internal class PluginService
    {
        readonly string PluginPath = "plugin";

        List<TabItem> _tabitems = new List<TabItem>();

        Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

        ConfigService configService = Ioc.Default.GetRequiredService<ConfigService>();

        Version MainVersion = Assembly.GetEntryAssembly().GetName().Version;

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
                    Model.Plugin pluginConfig = configService.GetPlugin(exp.FullName);
                    if(pluginConfig == null)
                    {
                        pluginConfig = new Model.Plugin(exp.FullName, true);
                        configService.AddPlugin(pluginConfig);
                    }

                    WinVPNPlugin obj = (WinVPNPlugin)plugin.CreateInstance(exp.FullName);
                    obj.IsEnable = pluginConfig.IsEnabled;

                    IPlugin p = (IPlugin)obj;
                    if(MainVersion < p.MiniDependentVersion)
                    {
                        obj.IsEnable = false;
                        _plugins.Add(exp.FullName, p);
                        continue;
                    }
                    _plugins.Add(exp.FullName, p);
                    
                    if(obj.IsEnable)
                    {
                        this._GetMainWindowTabItems(exp, obj);
                        this._GetMainWindowLeftCommands(exp, obj);
                        this._GetMainWindowRightCommands(exp, obj);
                        this._GetMainWindowStatusBarItems(exp, obj);
                    }
                    break;
                }
            }
            configService.Save();
        }

        public void UpdatePluginConfig(WinVPNPlugin plugin)
        {
            Model.Plugin p = configService.GetPlugin(plugin.Name);
            p.IsEnabled = plugin.IsEnable;
            configService.Save();
        }

        private void _GetMainWindowTabItems(Type type, object obj)
        {
            MethodInfo GetMainWindowTabItems = type.GetMethod("GetMainWindowTabItems");
            if (GetMainWindowTabItems != null)
            {
                IEnumerable<TabItem> items = (TabItem[])GetMainWindowTabItems.Invoke(obj, null);
                if (items != null)
                {
                    foreach (TabItem tab in items)
                    {
                        tab.Tag = type.FullName;
                    }
                    _tabitems.AddRange(items);
                }
            }
        }

        private void _GetMainWindowLeftCommands(Type type, object obj)
        {
            MethodInfo GetMainWindowLeftCommands = type.GetMethod("GetMainWindowLeftCommands");
            if (GetMainWindowLeftCommands != null)
            {
                IEnumerable<FrameworkElement> items = (FrameworkElement[])GetMainWindowLeftCommands.Invoke(obj, null);
                if (items != null)
                {
                    foreach (FrameworkElement tab in items)
                    {
                        tab.Tag = type.FullName;
                    }
                }
            }
        }

        private void _GetMainWindowRightCommands(Type type, object obj)
        {
            MethodInfo GetMainWindowRightCommands = type.GetMethod("GetMainWindowRightCommands");
            if (GetMainWindowRightCommands != null)
            {
                IEnumerable<FrameworkElement> items = (FrameworkElement[])GetMainWindowRightCommands.Invoke(obj, null);
                if (items != null)
                {
                    foreach (FrameworkElement tab in items)
                    {
                        tab.Tag = type.FullName;
                    }
                }
            }
        }

        private void _GetMainWindowStatusBarItems(Type type, object obj)
        {
            MethodInfo _GetMainWindowStatusBarItems = type.GetMethod("_GetMainWindowStatusBarItems");
            if (_GetMainWindowStatusBarItems != null)
            {
                IEnumerable<StatusBarItem> items = (StatusBarItem[])_GetMainWindowStatusBarItems.Invoke(obj, null);
                if (items != null)
                {
                    foreach (StatusBarItem tab in items)
                    {
                        tab.Tag = type.FullName;
                    }
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
