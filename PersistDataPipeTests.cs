using DaemonTechChallenge.Data.Connection;
using DaemonTechChallenge.ETL.Pipes;
using Moq;
using System.Data;

namespace DaemonTechChallengeTests;

public class PersistDataPipeTests
{
    [Fact]
    public async Task CreateStoreBlock_SavesDataToDatabase()
    {
        // Arrange
        var mockConnection = new Mock<IDBConnection>();
        mockConnection.Setup(c => c.Open()).Returns(true);
        mockConnection.Setup(c => c.SaveAsync(It.IsAny<DataTable>(), It.IsAny<string>())).ReturnsAsync(true);

        var persistDataPipe = new PersistDataPipe(mockConnection.Object);
        var block = persistDataPipe.CreateStoreBlock("TestTable");

        // Act
        var dataTable = new DataTable();
        dataTable.Columns.Add("Id", typeof(int));
        dataTable.Columns.Add("Name", typeof(string));
        dataTable.Rows.Add(1, "Test 1");
        dataTable.Rows.Add(2, "Test 2");

        block.Post(dataTable);
        block.Complete();
        await block.Completion;

        // Assert
        mockConnection.Verify(c => c.Open(), Times.Once);
        mockConnection.Verify(c => c.SaveAsync(dataTable, "TestTable"), Times.Once);
    }

    [Fact]
    public async Task CreateStoreBlock_ClosesConnectionOnCompletion()
    {
        // Arrange
        var mockConnection = new Mock<IDBConnection>();
        mockConnection.Setup(c => c.Open()).Returns(true);
        mockConnection.Setup(c => c.SaveAsync(It.IsAny<DataTable>(), It.IsAny<string>())).ReturnsAsync(true);

        var persistDataPipe = new PersistDataPipe(mockConnection.Object);
        var block = persistDataPipe.CreateStoreBlock("TestTable");

        // Act
        block.Complete();
        await block.Completion;
        await Task.Delay(100);

        // Assert
        mockConnection.Verify(c => c.Close(), Times.Once);
    }
}
