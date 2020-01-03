using CommandLine;
using System;
using System.IO;
using System.Reflection;
using TaskIt.NexusUploader.Options;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    class Program
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
                errs => new Result(EExitCode.SUCCESS, ""));

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
            var filePaths = GetFilePaths(options.SourceFolder, out var ret);
            if (ret != null)
            {
                return ret;
            }

            // http client initialisieren
            var uploader = new HttpUploader(options);
            // upload
            ret = uploader.UploadAsync(filePaths).GetAwaiter().GetResult();
            if (ret != null)
            {
                ret = uploader.RemoveAsync(filePaths).GetAwaiter().GetResult();
            }
            return ret;
        }



        /// <summary>
        /// gets all file pathes
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string[] GetFilePaths(string path, out Result result)
        {
            result = null;
            if (string.IsNullOrEmpty(path))
            {
                path = Environment.CurrentDirectory;
            }
            string[] filePaths = null;
            try
            {
                filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                result = new Result(EExitCode.INVALID_FOLDER, path);
            }

            return filePaths;
        }
    }
}
