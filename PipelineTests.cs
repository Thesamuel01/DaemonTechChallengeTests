using DaemonTechChallenge.ETL;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallengeTests;

public class PipelineTests
{
    [Fact]
    public async Task Startup_PassesDataThroughPipeline()
    {
        // Arrange
        var bufferBlock = new BufferBlock<int>();
        var pipeline = new Pipeline(bufferBlock, null);

        var transformBlock = new TransformBlock<int, int>(x => x * 2);
        pipeline.AddStageToPipeline(transformBlock);

        var actionBlock = new ActionBlock<int>(x => Assert.Equal(4, x));
        pipeline.AddStageToPipeline(actionBlock);

        // Act
        _ = pipeline.Startup(2);
        actionBlock.Complete();
        await actionBlock.Completion;

        // Assert
        Assert.True(actionBlock.Completion.IsCompleted);
    }


    [Fact]
    public void AddStageToPipeline_LinksBlocks()
    {
        // Arrange
        var bufferBlock = new BufferBlock<int>();
        var pipeline = new Pipeline(bufferBlock, null);

        var transformBlock = new TransformBlock<int, int>(x => x * 2);
        pipeline.AddStageToPipeline(transformBlock);

        _ = pipeline.Startup(2);

        // Act
        int result;
        transformBlock.OutputAvailableAsync().Wait();
        transformBlock.TryReceive(out result);

        // Assert
        Assert.Equal(4, result);
    }
}
