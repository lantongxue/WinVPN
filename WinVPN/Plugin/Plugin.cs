using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WinVPN.Plugin
{
    public abstract class Plugin
    {
        /// <summary>
        /// 插件名字
        /// </summary>
        public abstract string PluginName { get; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public abstract string PluginVersion { get; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public abstract string PluginAuthor { get; }

        /// <summary>
        /// 插件项目主页
        /// </summary>
        public abstract string PluginWebsite { get; }

        /// <summary>
        /// 是否支持附加设置
        /// </summary>
        public abstract bool IsSupportSettings { get; }

        public virtual IEnumerable<TabItem> GetTabItems()
        {
            return new List<TabItem>();
        }

        public virtual void Settings()
        {
        }
    }
}
