using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
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

        Dictionary<string, WinVPN_Plugin> _plugins = new Dictionary<string, WinVPN_Plugin>();

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
            try
            {
                Assembly plugin = Assembly.LoadFile(pluginPath);
                foreach (Type exp in plugin.ExportedTypes)
                {
                    if (exp.GetInterface("WinVPN.Plugin.SDK.IWinVPNPlugin") != null)
                    {
                        IWinVPNPlugin obj = (IWinVPNPlugin)plugin.CreateInstance(exp.FullName);

                        WinVPN_Plugin _plugin = new WinVPN_Plugin(obj)
                        {
                            Name = exp.FullName,
                            IsEnabled = MainVersion >= obj.MiniDependentVersion,
                        };
                        _plugin.IsOn = _plugin.IsEnabled;

                        bool? isEnabled = configService.GetPlugin(exp.FullName);
                        if (isEnabled == null)
                        {
                            configService.AddPlugin(exp.FullName, _plugin.IsOn);
                        }
                        else
                        {
                            _plugin.IsOn = isEnabled.Value;
                        }

                        _plugins.Add(exp.FullName, _plugin);
                        break;
                    }
                }
                configService.Save();
            }
            catch
            {

            }
        }

        public void UpdatePluginConfig(WinVPN_Plugin plugin)
        {
            configService.SetPlugin(plugin.Name, plugin.IsOn);
            configService.Save();
        }

        public Dictionary<string, WinVPN_Plugin> GetPlugins()
        {
            return _plugins;
        }
    }
}
