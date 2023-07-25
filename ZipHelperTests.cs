using System.IO.Compression;
using System.Text;
using DaemonTechChallenge.Helpers;

namespace DaemonTechChallengeTests;

public class ZipHelperTests
{
    [Fact]
    public void Read_ReturnsCorrectFiles()
    {
        // Arrange
        var zipStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var entry1 = zipArchive.CreateEntry("file1.txt");
            using (var writer = new StreamWriter(entry1.Open()))
            {
                writer.Write("File Content 1");
            }

            var entry2 = zipArchive.CreateEntry("file2.txt");
            using (var writer = new StreamWriter(entry2.Open()))
            {
                writer.Write("File Content 2");
            }
        }
        zipStream.Seek(0, SeekOrigin.Begin);

        var zipHelper = new ZipHelper();

        // Act
        var files = zipHelper.Read(zipStream, ".txt");

        // Assert
        Assert.Equal(2, files.Count);
        Assert.Equal("File Content 1", Encoding.UTF8.GetString(files[0].ToArray()));
        Assert.Equal("File Content 2", Encoding.UTF8.GetString(files[1].ToArray()));
    }
}
