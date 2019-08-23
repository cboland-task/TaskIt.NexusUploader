using System.Collections.Generic;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
   public class UploaderOptions
    {
        public static readonly Dictionary<ParamType, string> ParamKeys = new Dictionary<ParamType, string> {
            { ParamType.USERNAME, "-u" },
            { ParamType.PASSWORD, "-p" },
            { ParamType.REPO_URL, "-t" },
            { ParamType.SOURCE, "-s" },
            { ParamType.GROUP, "-g" },
            { ParamType.ARTIFACT, "-a" },
            { ParamType.VERSION, "-v" }
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
