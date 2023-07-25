using DaemonTechChallenge.ETL.Pipes;
using DaemonTechChallenge.Helpers;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallengeTests;

public class FetchZipDataPipeTests
{
    [Fact]
    public void CreateFetchZipDataBlock_ReturnsTransformManyBlock()
    {
        // Arrange
        var mockZiperHelper = new Mock<IZiperHelper>();
        var mockHttpClient = new HttpClient();
        var fetchZipDataPipe = new FetchZipDataPipe(mockZiperHelper.Object, mockHttpClient);

        // Act
        var result = fetchZipDataPipe.CreateFetchZipDataBlock(".txt");

        // Assert
        Assert.IsType<TransformManyBlock<string, MemoryStream>>(result);
    }

    [Fact]
    public async Task CreateFetchZipDataBlock_DownloadsAndReadsData()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{'id':1,'value':'1'}]"),
            })
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object);
        var mockZiperHelper = new Mock<IZiperHelper>();
        mockZiperHelper.Setup(z => z.Read(It.IsAny<Stream>(), It.IsAny<string>()))
            .Returns(new MemoryStream[] { new MemoryStream(new byte[] { 1, 2, 3 }) }.ToList());

        var fetchZipDataPipe = new FetchZipDataPipe(mockZiperHelper.Object, httpClient);
        var block = fetchZipDataPipe.CreateFetchZipDataBlock(".txt");

        // Act
        block.Post("https://example.com/data.zip");
        block.Complete();
        var result = await block.ReceiveAsync();

        // Assert
        Assert.Equal(new byte[] { 1, 2, 3 }, result.ToArray());
    }
}
