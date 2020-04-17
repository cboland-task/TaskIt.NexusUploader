using System;
using TaskIt.NexusUploader.Options;
using Xunit;

namespace TaskIt.NexusUploader.Test
{
    public class HttpUploaderTest
    {
        private HttpUploader systemUnderTest;
        private UploaderOptions sourceOptions = new UploaderOptions() { RepositoryUrl = "http://www.bing.de/", ArtifactId = "NexusUploader", GroupId = "TaskIt", Revision = "1.0.0" };


        /// <summary>
        /// Unit test for <see cref="HttpUploader.UploadAsync(string[])"/>
        /// </summary>
        [Fact]
        public void TestConstruction()
        {
            sourceOptions = new UploaderOptions();
            Assert.Throws<UriFormatException>(() => new HttpUploader(sourceOptions));

        }

        /// <summary>
        /// Unit test for <see cref="HttpUploader.UploadAsync(string[])"/>
        /// </summary>
        [Fact]
        public void TestUploadAsyncEmptyFiles()
        {
            systemUnderTest = new HttpUploader(sourceOptions);

            var result = systemUnderTest.UploadAsync(null).GetAwaiter().GetResult();
            Assert.True(result == null, "Result is not null");
        }

        /// <summary>
        /// Unit test for <see cref="HttpUploader.UploadAsync(string[])"/>
        /// </summary>
        [Fact]
        public void TestUploadAsyncSomeFiles()
        {
            // init Test            
            systemUnderTest = new HttpUploader(sourceOptions);

            var sourceFiles = Filehelper.GetFilePaths(sourceOptions.SourceFolder, out var resultMessage);

            // perform action
            var result = systemUnderTest.UploadAsync(sourceFiles).GetAwaiter().GetResult();

            // check result
            Assert.True(result.Code == Types.EExitCode.UPLOAD_ERROR, "Unexpected Upload Success");
        }

        /// <summary>
        /// Unit test for <see cref="HttpUploader.ConstructUrl(string, System.Net.Http.HttpClient)"/>
        /// </summary>
        [Theory]
        [InlineData(@"c:\Temp\test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test.txt")]
        [InlineData(@"c:\Temp\\test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test.txt")]
        [InlineData(@"c:\Temp/test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test.txt")]
        [InlineData(@"c:\Temp//test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test.txt")]
        [InlineData(@"c:\Temp\test 2.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test 2.txt")]
        [InlineData(@"c:\Temp\test\test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test/test.txt")]
        [InlineData(@"c:\Temp\test\test\test.txt", "http://www.bing.de/TaskIt/NexusUploader/1.0.0/test/test/test.txt")]

        public void TestConstructUrl(string source, string expected)
        {
            // init Test            
            sourceOptions.SourceFolder = @"c:\Temp";

            systemUnderTest = new HttpUploader(sourceOptions);
            var result = systemUnderTest.ConstructUrl(source);

            Assert.Equal(expected, result.ToString());
        }

        /// <summary>
        /// Unit test for <see cref="HttpUploader.RemoveAsync(string[])"/>
        /// </summary>
        [Fact]
        public void TestRemoveAsyncEmptyFiles()
        {
            systemUnderTest = new HttpUploader(sourceOptions);

            var result = systemUnderTest.RemoveAsync(null).GetAwaiter().GetResult();
            Assert.True(result == null, "Unexpected Result");
        }

        /// <summary>
        /// Unit test for <see cref="HttpUploader.RemoveAsync(string[])"/>
        /// </summary>
        [Fact]
        public void TestRemoveAsyncSomeFiles()
        {
            // init Test            
            systemUnderTest = new HttpUploader(sourceOptions);

            var sourceFiles = Filehelper.GetFilePaths(sourceOptions.SourceFolder, out var resultMessage);

            // perform action
            var result = systemUnderTest.RemoveAsync(sourceFiles).GetAwaiter().GetResult();

            // check result
            Assert.True(result == null, "Unexpected Result");
        }
    }
}
