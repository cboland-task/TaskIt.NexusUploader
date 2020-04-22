using System;
using System.IO;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    /// <summary>
    /// Utility for File Handling
    /// </summary>
    public static class Filehelper
    {
        /// <summary>
        /// gets all files in all subriectories
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string[] GetFilePaths(string path, out Result result)
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
            catch (Exception)
            {
                result = new Result(EExitCode.INVALID_FOLDER, $"Check your path: {path}");
            }

            return filePaths;
        }
    }
}
