using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskIt.NexusUploader.Options;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    public class HttpUploader
    {
        /// <summary>
        /// Trenner fuer den Bau von URLs
        /// </summary>
        private const string URL_DELIMETER = "/";

        /// <summary>
        /// Uploader options
        /// </summary>
        private UploaderOptions _options;

        /// <summary>
        /// Http client - delgate
        /// </summary>
        private HttpClient _httpCLient;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="options"></param>
        public HttpUploader(UploaderOptions options)
        {
            _options = options;
            _httpCLient = InitHttpClient(_options);
        }

        /// <summary>
        /// Initialisiert den HTTP Client
        /// </summary>
        /// <param name="options"></param>
        /// <returns>HttpClient</returns>
        private HttpClient InitHttpClient(UploaderOptions options)
        {
            HttpClient ret = null;
            ret = new HttpClient
            {
                BaseAddress = new Uri(options.RepositoryUrl + options.GroupId + URL_DELIMETER + options.ArtifactId + URL_DELIMETER + options.Revision)
            };

            string authparam = options.Username + ":" + options.Password;
            byte[] bytes = Encoding.UTF8.GetBytes(authparam);
            var base64String = Convert.ToBase64String(bytes);
            ret.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64String);

            return ret;
        }

        /// <summary>
        /// uploads all files
        /// </summary>        
        /// <param name="filePaths"></param>
        public async Task<Result> UploadAsync(string[] filePaths)
        {
            Result ret = null;
            filePaths = filePaths ?? Array.Empty<string>();

            int errorCount = 0;
            foreach (var item in filePaths)
            {
                byte[] fileContent = File.ReadAllBytes(item);
                var url = ConstructUrl(item);
                Console.Write($"Pushing: {item} --> {url.ToString()}");

                using (
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new ByteArrayContent(fileContent)
                })
                {
                    SetContentType(request, item);
                    try
                    {
                        using (var response = await _httpCLient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine(Messages.MSG_UPLOAD_Ok);
                            }
                            else
                            {
                                ret = new Result(EExitCode.UPLOAD_ERROR, response.ReasonPhrase);
                                Console.WriteLine(ret.ToString());
                                errorCount++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ret = new Result(EExitCode.UPLOAD_ERROR, e.Message);
                        Console.WriteLine(ret.ToString());
                        errorCount++;
                    }
                }
            }
            Console.WriteLine($"Finished Uploading {filePaths.Length} Files with {errorCount} Errors");

            return ret;
        }

        /// <summary>
        /// uploads all files
        /// </summary>        
        /// <param name="filePaths"></param>
        public async Task<Result> RemoveAsync(string[] filePaths)
        {
            Result ret = null;
            filePaths = filePaths ?? Array.Empty<string>();

            Console.WriteLine(Messages.MSG_ROLLBACK);
            foreach (var item in filePaths)
            {
                var url = ConstructUrl(item);
                Console.WriteLine($"Removing: {url.ToString()}");
                using (
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
                {

                    await _httpCLient.SendAsync(request).ConfigureAwait(false);
                }
            }

            Console.WriteLine(Messages.MSG_ROLLBACK_FINISH);
            return ret;
        }


        /// <summary>
        /// setzt den content type im Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filename"></param>
        private void SetContentType(HttpRequestMessage request, string filename)
        {
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(filename);
            request.Content.Headers.Add("Content-Type", mimeType);
        }

        /// <summary>
        /// generiert die Ziel URL
        /// </summary>
        /// <param name="filename"></param>       
        /// <returns></returns>
        public Uri ConstructUrl(string filename)
        {
            var relativePath = filename.Replace(_options.SourceFolder, "").Replace("\\", "/").Replace("//", "/");
            relativePath = Uri.EscapeUriString(relativePath);
            var result = new Uri(_httpCLient.BaseAddress.ToString() + relativePath);
            return result;
        }
    }
}
