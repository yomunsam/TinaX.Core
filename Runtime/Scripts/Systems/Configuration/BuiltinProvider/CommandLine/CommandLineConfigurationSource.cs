using System.Threading.Tasks;

namespace TinaX.Systems.Configuration.CommandLine
{
    /// <summary>
    /// Represents command line arguments as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class CommandLineConfigurationSource : IConfigurationSource
    {
        private ICommandLineArgs _args;

        public CommandLineConfigurationSource(ICommandLineArgs args)
        {
            _args = args;
        }

        public Task<IConfigurationProvider> BuildAsync(IConfigurationBuilder builder)
            => Task.FromResult<IConfigurationProvider>(new CommandLineConfigurationProvider(_args));
    }
}
