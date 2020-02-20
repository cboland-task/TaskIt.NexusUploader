using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskIt.NexusUploader.Options;
using TaskIt.NexusUploader.Types;

namespace TaskIt.NexusUploader
{
    class HttpUploader
    {
        /// <summary>
        /// Trenner fuer den Bau von URLs
        /// </summary>
        private const string URL_DELIMETER = "/";

        /// <summary>
        /// needet informations
        /// </summary>
        private readonly UploaderOptions _options;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="options"></param>
        public HttpUploader(UploaderOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Initialisiert den HTTP Client
        /// </summary>
        /// <param name="result"></param>
        /// <returns>HttpClient</returns>
        private HttpClient InitHttpClient(out Result result)
        {
            result = null;
            HttpClient ret = null;
            try
            {
                ret = new HttpClient
                {
                    BaseAddress = new Uri(_options.RepositoryUrl + _options.GroupId + URL_DELIMETER + _options.ArtifactId + URL_DELIMETER + _options.Revision)
                };

                string authparam = _options.Username + ":" + _options.Password;
                byte[] bytes = Encoding.UTF8.GetBytes(authparam);
                var base64String = Convert.ToBase64String(bytes);
                ret.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64String);
            }
            catch (Exception)
            {
                result = new Result(EExitCode.INVALID_PARAMS, $"Check {_options.RepositoryUrl} and {_options.GroupId } and {_options.ArtifactId} and {_options.Revision} ");
            }
            return ret;
        }

        /// <summary>
        /// uploads all files
        /// </summary>        
        /// <param name="filePaths"></param>
        public async Task<Result> UploadAsync(string[] filePaths)
        {
            Result ret = null;
            // init and use http client
            using (var client = InitHttpClient(out ret))
            {
                if (ret != null)
                {
                    ret.Code = EExitCode.UPLOAD_ERROR;
                    return ret;
                }
                int errorCount = 0;
                foreach (var item in filePaths)
                {
                    byte[] fileContent = File.ReadAllBytes(item);
                    var url = ConstructUrl(item, client);
                    Console.Write($"Pushing: {item} --> {url.ToString()}");

                    using (
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url)
                    {
                        Content = new ByteArrayContent(fileContent)
                    })
                    {
                        SetContentType(request, item);

                        using (var response = await client.SendAsync(request).ConfigureAwait(false))
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
                }
                Console.WriteLine($"Finished Uploading {filePaths.Length} Files with {errorCount} Errors");
            }
            return ret;
        }

        /// <summary>
        /// uploads all files
        /// </summary>        
        /// <param name="filePaths"></param>
        public async Task<Result> RemoveAsync(string[] filePaths)
        {
            Result ret = null;

            using (var client = InitHttpClient(out ret))
            {
                if (ret != null)
                {
                    return ret;
                }
                Console.WriteLine(Messages.MSG_ROLLBACK);
                foreach (var item in filePaths)
                {
                    var url = ConstructUrl(item, client);
                    Console.WriteLine($"Removing: {url.ToString()}");
                    using (
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
                    {

                        await client.SendAsync(request).ConfigureAwait(false);
                    }
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
        /// <param name="client"></param>
        /// <returns></returns>
        private Uri ConstructUrl(string filename, HttpClient client)
        {
            var relativePath = filename.Replace(_options.SourceFolder, "").Replace(" ", "").Replace("//", "");
            return new Uri(client.BaseAddress.ToString() + relativePath);
        }
    }
}
