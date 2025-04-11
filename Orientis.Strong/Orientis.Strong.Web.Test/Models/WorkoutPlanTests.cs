using Orientis.Strong.Web.Models;

namespace Orientis.Strong.Web.Test.Models;
public class WorkoutPlanTests
{
    [Fact]
    public void AddWorkout_ShouldAddWorkoutToWorkoutsCollection()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan("Push", DateTime.Now.AddDays(-7), DateTime.Now);
        var workout = new Workout(DateTime.Now.AddDays(-1), TimeSpan.FromHours(1));

        // Act
        workoutPlan.AddWorkout(workout);

        // Assert
        Assert.Contains(workout, workoutPlan.Workouts);
    }

    [Fact]
    public void AddWorkout_ShouldUpdateToDate_WhenWorkoutDateIsLater()
    {
        // Arrange
        var initialToDate = DateTime.Now;
        var workoutPlan = new WorkoutPlan("Push", DateTime.Now.AddDays(-7), initialToDate);
        var workout = new Workout(initialToDate.AddDays(1), TimeSpan.FromHours(1));

        // Act
        workoutPlan.AddWorkout(workout);

        // Assert
        Assert.Equal(workout.Date, workoutPlan.To);
    }

    [Fact]
    public void AddWorkout_ShouldNotUpdateToDate_WhenWorkoutDateIsEarlier()
    {
        // Arrange
        var initialToDate = DateTime.Now;
        var workoutPlan = new WorkoutPlan("Push", DateTime.Now.AddDays(-7), initialToDate);
        var workout = new Workout(initialToDate.AddDays(-1), TimeSpan.FromHours(1));

        // Act
        workoutPlan.AddWorkout(workout);

        // Assert
        Assert.Equal(initialToDate, workoutPlan.To);
    }

    [Fact]
    public void AddWorkout_ShouldThrowArgumentNullException_WhenWorkoutIsNull()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan("Push", DateTime.Now.AddDays(-7), DateTime.Now);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => workoutPlan.AddWorkout(null!));
    }
}
