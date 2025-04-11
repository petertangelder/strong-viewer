using Orientis.Strong.Web.Models;

namespace Orientis.Strong.Web.Test.Models;

public class WorkoutTests
{
    [Fact]
    public void AddSet_ShouldAddNewExercise_WhenExerciseDoesNotExist()
    {
        // Arrange
        var workout = new Workout(DateTime.Now, TimeSpan.FromMinutes(60));
        var set = new Set(1, 100, "kg", 10, null);

        // Act
        workout.AddSet("Bench Press", DateTime.Now, set);

        // Assert
        Assert.Single(workout.Exercises);
        var exercise = workout.Exercises.First();
        Assert.Equal("Bench Press", exercise.Name);
        Assert.Equal(set, exercise.Sets.First());
    }

    [Fact]
    public void AddSet_ShouldAddSetToExistingExercise_WhenExerciseExists()
    {
        // Arrange
        var workout = new Workout(DateTime.Now, TimeSpan.FromMinutes(60));
        var set1 = new Set(1, 100, "kg", 10, null);
        var set2 = new Set(2, 110, "kg", 8, null);
        var exerciseDate = DateTime.Now;

        workout.AddSet("Bench Press", exerciseDate, set1);

        // Act
        workout.AddSet("Bench Press", exerciseDate, set2);

        // Assert
        Assert.Single(workout.Exercises);
        var exercise = workout.Exercises.First();
        Assert.Equal(2, exercise.Sets.Count);
        Assert.Contains(set1, exercise.Sets);
        Assert.Contains(set2, exercise.Sets);
    }

    [Fact]
    public void AddSet_ShouldThrowArgumentException_WhenExerciseNameIsNullOrEmpty()
    {
        // Arrange
        var workout = new Workout(DateTime.Now, TimeSpan.FromMinutes(60));
        var set = new Set(1, 100, "kg", 10, null);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => workout.AddSet(null!, DateTime.Now, set));
        Assert.Throws<ArgumentException>(() => workout.AddSet(string.Empty, DateTime.Now, set));
    }

    [Fact]
    public void AddSet_ShouldThrowArgumentNullException_WhenSetIsNull()
    {
        // Arrange
        var workout = new Workout(DateTime.Now, TimeSpan.FromMinutes(60));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => workout.AddSet("Bench Press", DateTime.Now, null!));
    }
}
