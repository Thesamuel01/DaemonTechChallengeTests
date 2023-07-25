using DaemonTechChallenge;

namespace DaemonTechChallengeTests;

public class DataTableFormatHelperTests
{
    [Fact]
    public void Format_ShouldCreateDataTable_WithMembersOrder()
    {
        // Arrange
        var membersOrder = new[] { "Id", "Name", "Age" };
        var helper = new DataTableFormatHelper(membersOrder);

        var data = new List<Person>
        {
            new Person { Id = 1, Name = "Alice", Age = 30 },
            new Person { Id = 2, Name = "Bob", Age = 25 },
        };

        // Act
        var dataTable = helper.Format(data);

        // Assert
        Assert.NotNull(dataTable);
        Assert.Equal(3, dataTable.Columns.Count);
        Assert.Equal("Id", dataTable.Columns[0].ColumnName);
        Assert.Equal("Name", dataTable.Columns[1].ColumnName);
        Assert.Equal("Age", dataTable.Columns[2].ColumnName);

        Assert.Equal(2, dataTable.Rows.Count);
        Assert.Equal(1, dataTable.Rows[0]["Id"]);
        Assert.Equal("Alice", dataTable.Rows[0]["Name"]);
        Assert.Equal(30, dataTable.Rows[0]["Age"]);
        Assert.Equal(2, dataTable.Rows[1]["Id"]);
        Assert.Equal("Bob", dataTable.Rows[1]["Name"]);
        Assert.Equal(25, dataTable.Rows[1]["Age"]);
    }

    private class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
