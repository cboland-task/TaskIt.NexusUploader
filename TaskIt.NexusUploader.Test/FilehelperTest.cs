using System.Linq;
using Xunit;

namespace TaskIt.NexusUploader.Test
{
    public class FilehelperTest
    {
        /// <summary>
        /// Unit Test for <see cref="Filehelper.GetFilePaths(string, out Types.Result)"/>
        /// </summary>
        [Fact]
        public void TestGetFilePathsNull()
        {
            var resultFiles = Filehelper.GetFilePaths(null, out var resultMessage);
            Assert.True(resultMessage == null, $"Result is not null");
            Assert.True(resultFiles.Any(), "Expected at least on file");
        }

        /// <summary>
        /// Unit Test for <see cref="Filehelper.GetFilePaths(string, out Types.Result)"/>
        /// </summary>
        [Fact]
        public void TestGetFilePathsInvalidFolder()
        {
            var resultFiles = Filehelper.GetFilePaths("c:\testtesttest", out var resultMessage);
            Assert.True(resultMessage.Code == Types.EExitCode.INVALID_FOLDER, $"Unexpected Result. Expected {Types.EExitCode.INVALID_FOLDER}");
            Assert.True(resultFiles == null, "Expected empty Array");
        }
    }
}
