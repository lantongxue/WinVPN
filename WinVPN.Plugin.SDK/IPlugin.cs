using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Plugin.SDK
{
    public interface IPlugin
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
    }
}
