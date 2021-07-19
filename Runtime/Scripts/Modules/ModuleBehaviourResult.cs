using TinaX.Exceptions;

namespace TinaX.Modules
{
    public struct ModuleBehaviourResult
    {
        public string ModuleName { get; set; }
        public XException Exception { get; set; }
        public bool IsError { get; set; }

        

        public static ModuleBehaviourResult CreateSuccess(string moduleName)
        {
            return new ModuleBehaviourResult
            {
                IsError = false,
                ModuleName = moduleName
            };
        }
    }
}
