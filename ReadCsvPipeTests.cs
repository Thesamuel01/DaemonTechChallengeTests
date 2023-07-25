using DaemonTechChallenge.ETL.Pipes;
using DaemonTechChallenge.Helpers;
using Moq;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallengeTests;

public class ReadCsvPipeTests
{
    [Fact]
    public void CreateReadCsvBlock_ReturnsTransformManyBlock()
    {
        // Arrange
        var mockCsvHelper = new Mock<ICsvHelper>();
        var readCsvPipe = new ReadCsvPipe(mockCsvHelper.Object);

        // Act
        var result = readCsvPipe.CreateReadCsvBlock<TestData>();

        // Assert
        Assert.IsType<TransformManyBlock<Stream, TestData>>(result);
    }

    [Fact]
    public void CreateReadCsvBlock_ReadsDataFromStream()
    {
        // Arrange
        var mockCsvHelper = new Mock<ICsvHelper>();
        mockCsvHelper.Setup(c => c.ReadAsync<TestData>(It.IsAny<Stream>()))
            .ReturnsAsync(new List<TestData> { new TestData { Id = 1, Name = "Test 1" }, new TestData { Id = 2, Name = "Test 2" } });

        var readCsvPipe = new ReadCsvPipe(mockCsvHelper.Object);
        var block = readCsvPipe.CreateReadCsvBlock<TestData>();

        // Act
        IList<TestData>? list = new List<TestData>();
        block.Post(new MemoryStream());
        block.OutputAvailableAsync().Wait();
        var result = block.TryReceiveAll(out list);

        // Assert
        Assert.Equal(2, list?.Count);
        Assert.Equal(1, list?[0].Id);
        Assert.Equal("Test 1", list?[0].Name);
        Assert.Equal(2, list?[1].Id);
        Assert.Equal("Test 2", list?[1].Name);
    }


    private class TestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

