using System.Diagnostics.CodeAnalysis;

namespace TaskIt.NexusUploader.Types
{
    /// <summary>
    /// possible exit codes / errors
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
    public enum EExitCode
    {
        /// <summary>
        /// ok
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// illegal parameters
        /// </summary>
        INVALID_PARAMS = 1,

        /// <summary>
        /// illegal folder
        /// </summary>
        INVALID_FOLDER = 2,

        /// <summary>
        /// Error during upload
        /// </summary>
        UPLOAD_ERROR = 3
    }
}
