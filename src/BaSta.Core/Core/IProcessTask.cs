namespace BaSta.Core
{
    /// <summary>
    /// Interface for task objects with a separate process.
    /// </summary>
    public interface IProcessTask
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled and should be initialized.
        /// </summary>
        /// <value><c>true</c> if enabled, otherwise <c>false</c>.</value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized, otherwise <c>false</c>.</value>
        bool IsInitialized { get; }
        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        void Terminate();
    }
}