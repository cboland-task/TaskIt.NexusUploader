using System.Collections.Generic;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    /// <summary>
    /// holds the Parameters, needed for the tool to operate
    /// </summary>
    public class UploaderOptions
    {
        // Semantics to param string map
        public static readonly Dictionary<EParamType, string> ParamKeys = new Dictionary<EParamType, string> {
            { EParamType.USERNAME, "-u" },
            { EParamType.PASSWORD, "-p" },
            { EParamType.REPO_URL, "-t" },
            { EParamType.SOURCE_FOLDER, "-f" },
            { EParamType.GROUP, "-g" },
            { EParamType.ARTIFACT, "-a" },
            { EParamType.VERSION, "-v" }
        };
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepositoryUrl { get; set; }
        public string SourceFolder { get; set; }
        public string GroupId { get; set; }
        public string ArtifactId { get; set; }
        public string Revision { get; set; }
    }
}
