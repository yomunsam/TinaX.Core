namespace TinaX
{
    /// <summary>
    /// TinaX bootstrap interface
    /// </summary>
    public interface IXBootstrap
    {
        /// <summary>
        /// Invoke after framework's services init/register , and brfore framework's services start.
        /// </summary>
        void OnInit(IXCore core);

        /// <summary>
        /// Invoke after framework's services start.
        /// </summary>
        void OnStart(IXCore core);

        void OnQuit();

        void OnAppRestart();
    }
}