using System;
using TinaX.Container;


namespace TinaX.Options.Internal
{
    public class Options : IOptions
    {
        public const string DefaultName = "default";

        public object OptionValue
        {
            get
            {
                if(_optionValue == null)
                {
                    _optionValue = Core.CreateInstance(m_OptionType);
                    m_ConfigureOptions(_optionValue);
                }
                return _optionValue;
            }
        }


        private IXCore Core
        {
            get
            {
                if (_core == null)
                    _core = m_Services.Get<IXCore>();

                return _core;
            }
        }
        private IXCore _core;

        private readonly Action<object> m_ConfigureOptions;
        private readonly IServiceContainer m_Services;
        private readonly Type m_OptionType;

        private object _optionValue;

        public Options(Action<object> configureOptions, IServiceContainer services, Type optionType)
        {
            m_ConfigureOptions = configureOptions ?? throw new ArgumentNullException(nameof(configureOptions));
            m_Services = services ?? throw new ArgumentNullException(nameof(services));
            m_OptionType = optionType ?? throw new ArgumentNullException(nameof(optionType));
        } 


    }


    public class Options<T> : IOptions<T> where  T: class
    {
        private readonly Options m_Options;

        public T Value => (T)m_Options.OptionValue;

        public Options(Options options)
        {
            this.m_Options = options;
        }

    }
}

