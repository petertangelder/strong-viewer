using Orientis.Strong.Web.Models;

namespace Orientis.Strong.Web.Test.Models;

public class StrongDataTests
{
    [Fact]
    public void AddAllWorkouts_ShouldGroupWorkoutsByDate_AndCallPerformWorkoutForEachGroup()
    {
        // Arrange
        var strongData = new StrongData();
        var strongDataItems = new List<StrongDataItem>
        {
            new() { Date = new DateTime(2023, 1, 1), WorkoutName = "Workout A", ExerciseName = "Exercise 1", SetOrder = "1", Weight = 50, Reps = 10, WorkoutDuration = 3600 },
            new() { Date = new DateTime(2023, 1, 1), WorkoutName = "Workout A", ExerciseName = "Exercise 2", SetOrder = "2", Weight = 60, Reps = 8, WorkoutDuration = 3600 },
            new() { Date = new DateTime(2023, 1, 2), WorkoutName = "Workout B", ExerciseName = "Exercise 3", SetOrder = "1", Weight = 70, Reps = 6, WorkoutDuration = 3000 }
        };

        // Act
        strongData.AddAllWorkouts(strongDataItems);

        // Assert
        Assert.NotNull(strongData.WorkoutPlans);
        Assert.Equal(2, strongData.WorkoutPlans.Count); // Two distinct dates, so two workouts
        var firstWorkoutPlan = strongData.WorkoutPlans.First();
        Assert.Equal("Workout A", firstWorkoutPlan.Name);
        Assert.Single(firstWorkoutPlan.Workouts); // Only one workout for the first date
        var secondWorkoutPlan = strongData.WorkoutPlans.Last();
        Assert.Equal("Workout B", secondWorkoutPlan.Name);
        Assert.Single(secondWorkoutPlan.Workouts); // Only one workout for the second date
    }

    [Fact]
    public void GetExercisesFromWorkoutPlan_ShouldReturnAllExercisesFromWorkouts()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan("Test Plan", DateTime.Now.AddDays(-10), DateTime.Now);
        var workout1 = new Workout(DateTime.Now.AddDays(-10), TimeSpan.FromMinutes(60));
        var workout2 = new Workout(DateTime.Now.AddDays(-5), TimeSpan.FromMinutes(45));

        var exercise1 = new Exercise("Bench Press", DateTime.Now.AddDays(-10));
        var exercise2 = new Exercise("Squat", DateTime.Now.AddDays(-5));

        workout1.AddSet("Bench Press", DateTime.Now.AddDays(-10), new Set(1, 100, "kg", 10, null));
        workout2.AddSet("Squat", DateTime.Now.AddDays(-5), new Set(1, 120, "kg", 8, null));

        workout1.Exercises.ToList().Add(exercise1);
        workout2.Exercises.ToList().Add(exercise2);

        workoutPlan.AddWorkout(workout1);
        workoutPlan.AddWorkout(workout2);

        // Act
        var exercises = StrongData.GetExercisesFromWorkoutPlan(workoutPlan);

        // Assert
        Assert.NotNull(exercises);
        Assert.Equal(2, exercises.Count());
        Assert.Contains(exercises, e => e.Name == "Bench Press");
        Assert.Contains(exercises, e => e.Name == "Squat");
    }

    [Fact]
    public void GetAllExercisesByName_ShouldReturnAllExercisesWithMatchingName()
    {
        // Arrange
        var strongData = new StrongData();
        var workoutPlan1 = new WorkoutPlan("Plan A", DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-20));
        var workoutPlan2 = new WorkoutPlan("Plan B", DateTime.Now.AddDays(-10), DateTime.Now);

        var workout1 = new Workout(DateTime.Now.AddDays(-30), TimeSpan.FromMinutes(60));
        var workout2 = new Workout(DateTime.Now.AddDays(-10), TimeSpan.FromMinutes(45));

        workout1.AddSet("Bench Press", DateTime.Now.AddDays(-30), new Set(1, 100, "kg", 10, null));
        workout1.AddSet("Squat", DateTime.Now.AddDays(-30), new Set(2, 120, "kg", 8, null));
        workout2.AddSet("Bench Press", DateTime.Now.AddDays(-10), new Set(1, 110, "kg", 12, null));

        workoutPlan1.AddWorkout(workout1);
        workoutPlan2.AddWorkout(workout2);

        strongData.AddAllWorkouts(new List<StrongDataItem>
        {
            new() { Date = DateTime.Now.AddDays(-30), WorkoutName = "Plan A", ExerciseName = "Bench Press", SetOrder = "1", Weight = 100, Reps = 10 },
            new() { Date = DateTime.Now.AddDays(-30), WorkoutName = "Plan A", ExerciseName = "Squat", SetOrder = "2", Weight = 120, Reps = 8 },
            new() { Date = DateTime.Now.AddDays(-10), WorkoutName = "Plan B", ExerciseName = "Bench Press", SetOrder = "1", Weight = 110, Reps = 12 }
        });

        // Act
        var exercises = strongData.GetAllExercisesByName("Bench Press");

        // Assert
        Assert.NotNull(exercises);
        Assert.Equal(2, exercises.Count());
        Assert.All(exercises, e => Assert.Equal("Bench Press", e.Name));
    }
}
