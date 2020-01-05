namespace TinaX
{
    /// <summary>
    /// TinaX bootstrap interface
    /// </summary>
    public interface IXBootstrap
    {
        /// <summary>
        /// Invoke before framework's services init and start.
        /// </summary>
        void OnInit();

        /// <summary>
        /// Invoke after framework's services start.
        /// </summary>
        void OnStart();

        void OnQuit();

        void OnAppRestart();
    }
}