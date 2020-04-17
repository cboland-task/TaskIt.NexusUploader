using CommandLine;
using System;
using System.Reflection;
using TaskIt.NexusUploader.Options;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    public class Program
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private Program()
        {
            // do nothing, just hide Construction

        }

        /// <summary>
        /// Main Methode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args)
        {
            int exitCode = (int)EExitCode.SUCCESS;

            var versionString = Assembly.GetEntryAssembly()
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion
                                        .ToString();

            Console.WriteLine($"NexusUploader {versionString} start...");

            // parse args           
            var ret = Parser.Default.ParseArguments<UploaderOptions>(args).MapResult(
                (UploaderOptions opts) => PerformAction(opts),
                errs => new Result(EExitCode.INVALID_PARAMS, ""));

            if (ret != null && ret.Code != EExitCode.SUCCESS)
            {
                Console.WriteLine($"ERROR: {ret.ToString()}");
                exitCode = (int)ret.Code;
            }

            Console.WriteLine($"NexusUploader {versionString} finished");
            return exitCode;
        }

        /// <summary>
        /// performs the real action
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static Result PerformAction(UploaderOptions options)
        {
            // filenamen lesen
            var filePaths = Filehelper.GetFilePaths(options.SourceFolder, out var ret);
            if (ret != null)
            {
                return ret;
            }

            // http client initialisieren
            HttpUploader uploader;
            try
            {
                uploader = new HttpUploader(options);
            }
            catch (Exception)
            {
                ret = new Result(EExitCode.INVALID_PARAMS, $"Check {options.RepositoryUrl} and {options.GroupId } and {options.ArtifactId} and {options.Revision} ");
                return ret;
            }

            // upload
            ret = uploader.UploadAsync(filePaths).GetAwaiter().GetResult();
            if (ret != null)
            {
                // remove uploaded files
                ret = uploader.RemoveAsync(filePaths).GetAwaiter().GetResult();
            }
            return ret;
        }




    }
}
