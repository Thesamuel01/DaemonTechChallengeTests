using System.Globalization;
using CsvHelper.Configuration;
using DaemonTechChallenge.Helpers;

namespace DaemonTechChallengeTests;

public class CsvHelperLibTests
{
    [Fact]
    public async Task ReadAsync_ShouldReadCsvRecords()
    {
        // Arrange
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        var csvHelper = new CsvHelperLib(csvConfig, new TestDataRecordMap());

        var csvData = @"ID;NAME
1;Test 01
2;Test 02
";

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, leaveOpen: true);
        await writer.WriteAsync(csvData);
        writer.Flush();
        stream.Position = 0;

        // Act
        var records = await csvHelper.ReadAsync<TestData>(stream);

        Assert.IsType<List<TestData>>(records);

        // Assert
        Assert.Equal(2, records.Count);
        Assert.Equal(1, records[0].Id);
        Assert.Equal("Test 01", records[0].Name);
    }

    private class TestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private class TestDataRecordMap : ClassMap<TestData>
    {
        public TestDataRecordMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.Name).Name("NAME");
        }
    }

}
