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
            argMap = null;
            // FPL Check
            if (args == null)
            {
                return EExitCode.INVALID_PARAMS;
            }

            // convert args to key and param list (odd / even)
            var values = args.Where((c, i) => i % 2 != 0).ToList();
            var keys = args.Where((c, i) => i % 2 == 0).ToList();

            // check number of args
            if (UploaderOptions.ParamKeys.Count != keys.Count || UploaderOptions.ParamKeys.Count != values.Count)
            {
                return EExitCode.INVALID_PARAMS;
            }

            // check if all arg keys are present
            if (keys.Except(UploaderOptions.ParamKeys.Values).Any())
            {
                return EExitCode.INVALID_PARAMS;
            }

            // build dictionary
            argMap = new Dictionary<string, string>();
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
                ArtifactId = source[UploaderOptions.ParamKeys[EParamType.ARTIFACT]],
                GroupId = source[UploaderOptions.ParamKeys[EParamType.GROUP]],
                Password = source[UploaderOptions.ParamKeys[EParamType.PASSWORD]],
                RepositoryUrl = source[UploaderOptions.ParamKeys[EParamType.REPO_URL]],
                Revision = source[UploaderOptions.ParamKeys[EParamType.VERSION]],
                SourceFolder = source[UploaderOptions.ParamKeys[EParamType.SOURCE_FOLDER]],
                Username = source[UploaderOptions.ParamKeys[EParamType.USERNAME]]
            };

            return ret;
        }
    }
}
