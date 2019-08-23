using System;
using System.IO;
using System.Reflection;
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
            var versionString = Assembly.GetEntryAssembly()
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion
                                        .ToString();

            Console.WriteLine($"NexusUploader {versionString} start...");

            // Parameter parsen
            var ret = new ArgParser().Parse(args, out UploaderOptions options);
            if (ret != EExitCode.SUCCESS)
            {
                PrintErrors(ret, options);
                return (int)ret;
            }

            // filenamen lesen
            var filePaths = GetFilePaths(options.SourceFolder, ref ret);
            if (ret != EExitCode.SUCCESS)
            {
                PrintErrors(ret, options);
                return (int)ret;
            }

            // http client initialisieren
            var uploader = new HttpUploader(options);
            // upload
            ret = uploader.UploadAsync(filePaths).GetAwaiter().GetResult();
            if (ret != EExitCode.SUCCESS)
            {
                ret = uploader.RemoveAsync(filePaths).GetAwaiter().GetResult();
            }
            PrintErrors(ret, options);

            Console.WriteLine($"NexusUploader {versionString} finished");
            return (int)ret;
        }



        /// <summary>
        /// Error ausgabe
        /// </summary>
        /// <param name="source"></param>
        /// <param name="options"></param>
        static void PrintErrors(EExitCode source, UploaderOptions options)
        {
            switch (source)
            {
                case EExitCode.SUCCESS:
                    break;
                case EExitCode.INVALID_PARAMS:
                    Console.WriteLine("ERROR - Invalid Parameters");
                    Console.WriteLine("Expected:");
                    foreach (var item in UploaderOptions.ParamKeys)
                    {
                        Console.WriteLine($"{item.Value} : {item.Key.ToString()}");
                    }
                    break;
                case EExitCode.INVALID_FOLDER:
                    Console.WriteLine("ERROR - Invalid Folder");
                    Console.WriteLine($"Folder does not exist: {options.SourceFolder}");
                    break;
                case EExitCode.UPLOAD_ERROR:
                    Console.WriteLine("ERROR - Upload Failed");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// gets all file pathes
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string[] GetFilePaths(string path, ref EExitCode result)
        {
            result = EExitCode.SUCCESS;
            string[] filePaths = null;
            try
            {
                filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                result = EExitCode.INVALID_FOLDER;
            }

            return filePaths;
        }
    }
}
