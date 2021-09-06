using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinaX.Module;

namespace TinaX.Modules.Internal
{
    public class ModulesManager
    {
        public ModulesManager()
        {

        }

        private List<IModuleProvider> m_Providers = new List<IModuleProvider>();
        private Dictionary<int, List<IModuleProvider>> m_OrderProviders = new Dictionary<int, List<IModuleProvider>>();

        public IList<IModuleProvider> Providers => m_Providers;
        public IDictionary<int, List<IModuleProvider>> OrderProviders => m_OrderProviders;


        public void Add(IModuleProvider provider)
        {
            if (m_Providers.Contains(provider))
                return;
            m_Providers.Add(provider);
            var order = GetModuleProviderOrder(ref provider);

            if (!m_OrderProviders.ContainsKey(order))
                m_OrderProviders.Add(order, new List<IModuleProvider>());
            if (!m_OrderProviders[order].Contains(provider))
                m_OrderProviders[order].Add(provider);
        }


        public void Sort()
        {
            m_Providers.Sort((x, y) => GetModuleProviderOrder(ref x).CompareTo(GetModuleProviderOrder(ref y)));
        }

        /// <summary>
        /// 获取模块的顺序
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public int GetModuleProviderOrder(ref IModuleProvider provider)
        {
            var attr = provider.GetType().GetCustomAttribute<ModuleProviderOrderAttribute>();
            return attr == null ? ModuleProviderOrderAttribute.DefaultOrder : attr.Order;
        }

        /// <summary>
        /// 获取经过排序的Order值的列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetOrderList()
        {
            var orders = m_OrderProviders.Select(op => op.Key).ToList();
            orders.Sort((x, y) => x.CompareTo(y));
            return orders;
        }
    }
}
