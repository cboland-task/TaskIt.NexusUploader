using System;
using Xunit;

namespace TaskIt.NexusUploader.Test
{
    /// <summary>
    /// Unit Test for <see cref="ArgParser"/>
    /// </summary>
    public class ArgParserTest
    {
        // object to test 
        private readonly ArgParser _testCandidate = new NexusUploader.ArgParser();

        /// <summary>
        /// Unit Test
        /// </summary>
        [Fact]
        public void Test1()
        {
            string[] testdata = new string[] { "a", "b" };
            var testResult = _testCandidate.Parse(testdata, out var options);

            Assert.True(testResult == Types.EExitCode.INVALID_PARAMS, "Wrong Exit Code");
            Assert.True(options == null, "Options Object is not null");
        }
    }
}
