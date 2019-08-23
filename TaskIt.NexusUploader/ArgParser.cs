using System.Collections.Generic;
using System.Linq;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    /// <summary>
    /// 
    /// </summary>
    public class ArgParser
    {
        public EExitCode Parse(string[] args, out UploaderOptions result)
        {
            EExitCode ret;
            result = null;
            ret = ConstructMap(args, out Dictionary<string, string> argMap);
            if (ret == EExitCode.SUCCESS)
            {
                result = CreateOptions(argMap);
            }
            return ret;
        }

        /// <summary>
        /// erzeugt eine Map aus dem Array
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private EExitCode ConstructMap(string[] args, out Dictionary<string, string> argMap)
        {
            var values = args.Where((c, i) => i % 2 != 0).ToList();
            var keys = args.Where((c, i) => i % 2 == 0).ToList();

            argMap = new Dictionary<string, string>();

            if (UploaderOptions.ParamKeys.Count != keys.Count || UploaderOptions.ParamKeys.Count != values.Count)
            {
                return EExitCode.INVALID_PARAMS;
                
            }

            for (int i = 0; i < keys.Count; i++)
            {
                argMap.Add(keys[i], values[i]);
            }

            return EExitCode.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private UploaderOptions CreateOptions(Dictionary<string, string> source)
        {
            UploaderOptions ret = new UploaderOptions()
            {
                ArtifactId = source[UploaderOptions.ParamKeys[ParamType.ARTIFACT]],
                GroupId = source[UploaderOptions.ParamKeys[ParamType.GROUP]],
                Password = source[UploaderOptions.ParamKeys[ParamType.PASSWORD]],
                RepositoryUrl = source[UploaderOptions.ParamKeys[ParamType.REPO_URL]],
                Revision = source[UploaderOptions.ParamKeys[ParamType.VERSION]],
                SourceFolder = source[UploaderOptions.ParamKeys[ParamType.SOURCE]],
                Username = source[UploaderOptions.ParamKeys[ParamType.USERNAME]]
            };

            return ret;
        }
    }
}
