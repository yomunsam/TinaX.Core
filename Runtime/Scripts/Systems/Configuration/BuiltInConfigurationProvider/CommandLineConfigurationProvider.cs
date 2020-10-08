namespace TinaX.Systems.Configuration
{
    /// <summary>
    /// 命令行 - 配置提供者
    /// </summary>
    internal class CommandLineConfigurationProvider : IConfigurationProvider
    {
        private ICommandLineArgs m_Args;

        public CommandLineConfigurationProvider(ICommandLineArgs args)
        {
            this.m_Args = args;
        }

        /// <summary>
        /// 获取String格式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetString(string key, out string value)
            => m_Args.TryGetValue(key, out value);
    }
}
