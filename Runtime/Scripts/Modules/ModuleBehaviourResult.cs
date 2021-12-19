using TinaX.Exceptions;

namespace TinaX.Modules
{
    public struct ModuleBehaviourResult
    {
#nullable enable
        public string ModuleName { get; set; }
        public XException? Exception { get; set; }
        public bool IsError { get; set; }

        

        public static ModuleBehaviourResult CreateSuccess(string moduleName)
        {
            return new ModuleBehaviourResult
            {
                IsError = false,
                ModuleName = moduleName
            };
        }

        public static ModuleBehaviourResult CreateFromException(string moduleName, XException exception)
        {
            return new ModuleBehaviourResult
            {
                IsError = true,
                ModuleName = moduleName,
                Exception = exception
            };
        }
#nullable restore
    }
}
