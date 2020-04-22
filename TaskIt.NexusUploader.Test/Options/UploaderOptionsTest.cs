using System;
using TaskIt.NexusUploader.Options;
using Xunit;

namespace TaskIt.NexusUploader.Test.Options
{
    public class UploaderOptionsTest
    {
        [Fact]
        public void TestConstruction()
        {
            // perform
            var result = new UploaderOptions();

            // test
            Assert.Equal(Environment.CurrentDirectory, result.SourceFolder);

        }
    }
}
