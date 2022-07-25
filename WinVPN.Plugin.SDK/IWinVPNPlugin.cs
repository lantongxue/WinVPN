using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WinVPN.Plugin.SDK
{
    public interface IWinVPNPlugin
    {
        /// <summary>
        /// 插件名字
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// 插件版本
        /// </summary>
        Version PluginVersion { get; }

        /// <summary>
        /// 插件作者
        /// </summary>
        string PluginAuthor { get; }

        /// <summary>
        /// 插件项目主页
        /// </summary>
        string PluginWebsite { get; }

        /// <summary>
        /// 是否支持附加设置
        /// </summary>
        bool IsSupportSettings { get; }

        /// <summary>
        /// 最低依赖（主程序）版本
        /// </summary>
        Version MiniDependentVersion { get; }

        void Settings();

        IEnumerable<TabItem> GetMainWindowTabItems();

        IEnumerable<FrameworkElement> GetMainWindowLeftCommands();

        IEnumerable<FrameworkElement> GetMainWindowRightCommands();

        IEnumerable<FrameworkElement> GetMainWindowStatusBarItems();
    }
}
