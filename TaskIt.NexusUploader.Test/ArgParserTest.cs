using System.Collections.Generic;
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
        /// generates a list of invalid parameter Testdata
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetInvalidArgs =>
        new List<object[]>
        {
            new object[] { new string[] { } },
            new object[] { new string[] { "a", "b", "C" } },
            new object[] { new string[] { "p1", "v1", "p2", "v2", "p3", "v3", "p4", "v4", "p5", "v5", "p6", "v6", "p7", "v7" } },
        };

        public static IEnumerable<object[]> GetValidArgs =>
        new List<object[]>
        {
            new object[] {
                new string[] {
                    UploaderOptions.ParamKeys[Types.EParamType.ARTIFACT], "v1",
                    UploaderOptions.ParamKeys[Types.EParamType.GROUP], "v2",
                    UploaderOptions.ParamKeys[Types.EParamType.PASSWORD], "v3",
                    UploaderOptions.ParamKeys[Types.EParamType.REPO_URL], "v4",
                    UploaderOptions.ParamKeys[Types.EParamType.VERSION], "v5",
                    UploaderOptions.ParamKeys[Types.EParamType.SOURCE_FOLDER], "v6",
                    UploaderOptions.ParamKeys[Types.EParamType.USERNAME], "v7" } },
        };

        /// <summary>
        /// Unit Test
        /// </summary>
        [Theory]
        [InlineData(null)]
        [MemberData(nameof(GetInvalidArgs))]
        public void TestInvalidParameters(string[] args)
        {
            var testResult = _testCandidate.Parse(args, out var options);

            Assert.True(testResult == Types.EExitCode.INVALID_PARAMS, "Wrong Exit Code");
            Assert.True(options == null, "Options Object is not null");
        }

        /// <summary>
        /// Unit Test
        /// </summary>
        [Theory]
        [MemberData(nameof(GetValidArgs))]
        public void TestVvalidParameters(string[] args)
        {
            var testResult = _testCandidate.Parse(args, out var options);

            Assert.True(testResult == Types.EExitCode.SUCCESS, "Wrong Exit Code");
            Assert.True(options != null, "Options Object is null");
        }
    }
}
