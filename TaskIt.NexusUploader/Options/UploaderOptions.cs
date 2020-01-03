using CommandLine;

namespace TaskIt.NexusUploader.Options

{
    /// <summary>
    /// holds the Parameters, needed for the tool to operate
    /// </summary>
    public class UploaderOptions
    {
        /// <summary>
        /// Nexus api username
        /// </summary>
        [Option('u', "user", Required = true, HelpText = "nexus upload api user")]
        public string Username { get; set; }

        /// <summary>
        /// nexus upload api password
        /// </summary>
        [Option('p', "password", Required = true, HelpText = "nexus upload api password")]
        public string Password { get; set; }

        /// <summary>
        /// Nexus Repository url
        /// </summary>
        [Option('t', "targetUrl", Required = true, HelpText = "nexus upload repository url")]
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// Source / root folder
        /// </summary>
        [Option('f', "folder", Required = false, HelpText = "local root folder to be uploaded")]
        public string SourceFolder { get; set; } = null;

        /// <summary>
        /// Artefact group id
        /// </summary>
        [Option('g', "groupId", Required = true, HelpText = "group id in the nexus repository")]
        public string GroupId { get; set; }

        /// <summary>
        /// Artifact id
        /// </summary>
        [Option('a', "artifactId", Required = true, HelpText = "artifact id in the nexus repository")]
        public string ArtifactId { get; set; }

        /// <summary>
        /// Artifact version
        /// </summary>
        [Option('v', "artifactVersion", Required = true, HelpText = "artifact version in the nexus repository")]
        public string Revision { get; set; }
    }
}
