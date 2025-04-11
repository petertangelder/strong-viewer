using System.Text;
using Orientis.Strong.Web.Services;

namespace Orientis.Strong.Web.Test.Services;

public class StrongImportServiceV6Tests
{
    [Fact]
    public async Task ImportStrongFileAsync_ShouldParseValidCsvFile()
    {
        // Arrange
        var csvContent = @"""Date"";""Workout Name"";""Exercise Name"";""Set Order"";""Weight (kg)"";""Seconds"";""Reps"";""Duration (sec)""
""2023-10-01 10:00:00"";""Push Day"";""Bench Press"";""1"";""100.5"";"""";""10"";""3600""
""2023-10-01 10:00:00"";""Push Day"";""Incline Dumbbell Press"";""2"";""40.0"";"""";""12"";""3600""
";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        var service = new StrongImportServiceV6();

        // Act
        var result = await service.ImportStrongFileAsync(stream);

        // Assert
        Assert.NotNull(result);
        var items = result.ToList();
        Assert.Equal(2, items.Count);

        Assert.Equal(new DateTime(2023, 10, 1, 10, 0, 0), items[0].Date);
        Assert.Equal("Push Day", items[0].WorkoutName);
        Assert.Equal("Bench Press", items[0].ExerciseName);
        Assert.Equal("1", items[0].SetOrder);
        Assert.Equal(100.5m, items[0].Weight);
        Assert.Null(items[0].Seconds);
        Assert.Equal(10, items[0].Reps);
        Assert.Equal(3600, items[0].WorkoutDuration);

        Assert.Equal(new DateTime(2023, 10, 1, 10, 0, 0), items[1].Date);
        Assert.Equal("Push Day", items[1].WorkoutName);
        Assert.Equal("Incline Dumbbell Press", items[1].ExerciseName);
        Assert.Equal("2", items[1].SetOrder);
        Assert.Equal(40.0m, items[1].Weight);
        Assert.Null(items[1].Seconds);
        Assert.Equal(12, items[1].Reps);
        Assert.Equal(3600, items[1].WorkoutDuration);
    }

    [Fact]
    public async Task ImportStrongFileAsync_ShouldThrowExceptionForMissingHeader()
    {
        // Arrange
        var csvContent = @"
""Workout Name"";""Exercise Name"";""Set Order"";""Weight (kg)"";""Seconds"";""Reps"";""Duration (sec)""
""Push Day"";""Bench Press"";""1"";""100.5"";"""";""10"";""3600""
";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        var service = new StrongImportServiceV6();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ImportStrongFileAsync(stream));
    }

    [Fact]
    public async Task ImportStrongFileAsync_ShouldHandleEmptyCsvFile()
    {
        // Arrange
        var csvContent = string.Empty;
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        var service = new StrongImportServiceV6();

        // Act
        var result = await service.ImportStrongFileAsync(stream);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}