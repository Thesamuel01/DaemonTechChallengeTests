using DaemonTechChallenge;
using DaemonTechChallenge.ETL.Pipes;
using DaemonTechChallenge.Helpers;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallengeTests;

public class ConvertToDataTablePipeTests
{
    private ConvertToDataTablePipe _pipe;
    private IDataTableFormat _dataTableFormat;

    public ConvertToDataTablePipeTests()
    {
        _dataTableFormat = new DataTableFormatHelper(new string[] { "Id", "Name" });
        _pipe = new ConvertToDataTablePipe(_dataTableFormat);
    }

    [Fact]
    public void TestCreateConvertToDataTableBlock()
    {
        var data = new List<TestData>
            {
                new TestData { Id = 1, Name = "Test 1" },
                new TestData { Id = 2, Name = "Test 2" }
            };

        var block = _pipe.CreateConvertToDataTableBlock<TestData>();
        block.Post(data);
        block.Complete();

        DataTable? result;
        block.OutputAvailableAsync().Wait();
        block.TryReceive(out result);

        Assert.NotNull(result);
        Assert.Equal(2, result.Rows.Count);
        Assert.Equal("Test 1", result.Rows[0]["Name"]);
        Assert.Equal("Test 2", result.Rows[1]["Name"]);
    }

    private class TestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

}
