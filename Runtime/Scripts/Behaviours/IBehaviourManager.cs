namespace TinaX.Core.Behaviours
{
    public interface IBehaviourManager
    {
        IAwakeBehaviour[] AwakeBehaviours { get; }
        IStartBehaviour[] StartBehaviours { get; }

        void RegisterObject(object obj);
        void Register(IAwakeBehaviour awakeBehaviour);
        void Register(IStartBehaviour startBehaviour);
        void RegisterAwake(IAwakeBehaviour awakeBehaviour);
        void RegisterStart(IStartBehaviour startBehaviour);
    }
}
