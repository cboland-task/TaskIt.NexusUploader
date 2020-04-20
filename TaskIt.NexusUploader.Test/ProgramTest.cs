using TaskIt.NexusUploader.Types;
using Xunit;

namespace TaskIt.NexusUploader.Test
{
    /// <summary>
    /// Unit Test fuer die Klasse <see cref="TaskIt.NexusUploader.Program"/>
    /// </summary>
    public class ProgramTest
    {

        /// <summary>
        /// Unit Test for <see cref="TaskIt.NexusUploader.Program.Main(string[])"/>
        /// </summary>
        [Fact]
        public void TestMainParseError()
        {
            // Init
            string[] sourceParams = { };
            // call
            int resultCode = Program.Main(sourceParams);

            // assert
            Assert.Equal((int)EExitCode.PARAM_PARSING_ERROR, resultCode);
        }

        /// <summary>
        /// Unit Test for <see cref="TaskIt.NexusUploader.Program.Main(string[])"/>
        /// </summary>
        [Fact]
        public void TestMainParamError()
        {
            // Init
            string[] sourceParams = { "-u", "test", "-p", "test", "-t", "http://www.bing.de", "-a", "NexusUploader", "-g", "TaskIt", "-v", "1.0.0" };
            // call
            int resultCode = Program.Main(sourceParams);

            // assert
            Assert.Equal((int)EExitCode.INVALID_PARAMS, resultCode);
        }

        /// <summary>
        /// Unit Test for <see cref="TaskIt.NexusUploader.Program.Main(string[])"/>
        /// </summary>
        [Fact]
        public void TestMainFolderError()
        {
            // Init
            string[] sourceParams = { "-u", "test", "-p", "test", "-t", "http://www.bing.de", "-a", "NexusUploader", "-g", "TaskIt", "-v", "1.0.0", "-f", @"v:\abcdef" };
            // call
            int resultCode = Program.Main(sourceParams);

            // assert
            Assert.Equal((int)EExitCode.INVALID_FOLDER, resultCode);
        }
    }
}
