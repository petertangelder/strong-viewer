using Radzen;

namespace Orientis.Strong.Web.Models;

/// <summary>
/// Domain class that transforms the list of <see cref="StrongDataItem"/> read from Strong export file into a 
/// list of workout plans, workouts, exercises and sets.
/// </summary>
public class StrongData
{
    public IReadOnlyCollection<WorkoutPlan> WorkoutPlans => _workoutPlans.AsReadOnly();

    private IList<WorkoutPlan> _workoutPlans = [];

    /// <summary>
    /// Processes the list of <see cref="StrongDataItem"/> read from Strong export file into a list of workout plans, workouts, exercises and sets.
    /// </summary>
    /// <param name="strongDataItems">An list of <see cref="StrongDataItem"/>, each item represents a single line in the Strong export file.</param>
    public void AddAllWorkouts(IEnumerable<StrongDataItem> strongDataItems)
    {
        ArgumentNullException.ThrowIfNull(strongDataItems, nameof(strongDataItems));

        if (!strongDataItems.Any())
        {
            throw new InvalidOperationException("No data items to process.");
        }

        _workoutPlans = [];
        var workouts = strongDataItems.GroupBy(x => x.Date);

        foreach (var workout in workouts)
        {
            PerformWorkout([.. workout]);
        }
    }

    /// <summary>
    /// Returns all exercises performed in all workouts performed in the given workout plan.
    /// </summary>
    /// <param name="workoutPlan">The <see cref="WorkoutPlan"/> to take all exercises from</param>
    /// <returns>A list of exercises, each exercise in the results
    /// represents an exercise from a single workout and contains an certain number of sets.</returns>
    public static IEnumerable<Exercise> GetExercisesFromWorkoutPlan(WorkoutPlan workoutPlan)
    {
        ArgumentNullException.ThrowIfNull(workoutPlan, nameof(workoutPlan));

        var workouts = workoutPlan.Workouts;

        return workouts.SelectMany(x => x.Exercises);
    }

    /// <summary>
    /// Returns all exercises of the given name performed in all workout plans. Usefull to see results over time for 
    /// a specific exercise like "Bench Press (Barbell)" or "Chest Fly (Dumbbell)".
    /// </summary>
    /// <param name="exerciseName">The name of the exercise to get all results from.</param>
    /// <returns>A list of exercises, each exercise in the results
    /// represents an exercise from a single workout and contains an certain number of sets.</returns>
    public IEnumerable<Exercise> GetAllExercisesByName(string exerciseName)
    {
        ArgumentException.ThrowIfNullOrEmpty(exerciseName, nameof(exerciseName));

        var exercises = new List<Exercise>();
        foreach (var workoutPlan in _workoutPlans)
        {
            foreach (var workout in workoutPlan.Workouts)
            {
                foreach (var exercise in workout.Exercises.Where(x => x.Name == exerciseName))
                {
                    exercises.Add(exercise);
                }
            }
        }
        return exercises;
    }

    /// <summary>
    /// Processes the results of a single workout. It skips lines with "Note" or "Rest Timer" in the "Set Order" column.
    /// </summary>
    /// <param name="strongDataItem">An instance of <see cref="StrongDataItem"/> that represents a single workout,
    /// items with the same date make up a single workout.</param>
    private void PerformWorkout(IEnumerable<StrongDataItem> allSetsInWorkout)
    {
        ArgumentNullException.ThrowIfNull(allSetsInWorkout, nameof(allSetsInWorkout));

        if (!allSetsInWorkout.Any())
        {
            throw new InvalidOperationException("Cannot perform a workout without data.");
        }

        // All items are of the same date, so we can take the first one to get the workout date and name.
        var firstItem = allSetsInWorkout.First();

        // Each workout is part of a plan, create it or get an existing one to which this workout can be added.
        var workoutPlan = CreateOrGetWorkoutPlan(firstItem.WorkoutName, firstItem.Date);

        // Start a new workout...
        var newWorkout = new Workout(firstItem.Date, TimeSpan.FromSeconds(firstItem.WorkoutDuration));

        // ... and perform all sets one by one
        foreach (var setItem in allSetsInWorkout)
        {
            if (!int.TryParse(setItem.SetOrder, out var setOrder))
            {
                // If the SetOrder is not a number, we assume it is something like a note or rest timer
                continue;
            }

            // Create a new set with the details of the set performed.
            var set = new Set(setOrder, setItem.Weight, "kg", setItem.Reps, (int?)setItem.Seconds);

            // Add the set to the workout.
            newWorkout.AddSet(setItem.ExerciseName, firstItem.Date, set);
        }

        // Add the workout to the workout plan.
        workoutPlan.AddWorkout(newWorkout);
    }

    /// <summary>
    /// Creates or gets a workout plan based on the name and date. If a workout plan with the same name already exists and the latest 
    /// workout is not older than 2 months, it returns the existing plan. Otherwise it creates a new one, adds it to the list and returns it.
    /// </summary>
    /// <param name="workoutName">The name of the workout plan like "Push" or "Fullbody A".</param>
    /// <param name="date">The date of workout.</param>
    /// <returns>A newly created or existing <see cref="WorkoutPlan"/> to add workouts to.</returns>
    private WorkoutPlan CreateOrGetWorkoutPlan(string workoutName, DateTime date)
    {
        ArgumentException.ThrowIfNullOrEmpty(workoutName, nameof(workoutName));

        var workoutPlan = _workoutPlans.LastOrDefault(x => x.Name == workoutName);

        if (workoutPlan == null || workoutPlan.To < date.AddMonths(-2))
        {
            workoutPlan = new WorkoutPlan(workoutName, date, date);
            _workoutPlans.Add(workoutPlan);
        }
        return workoutPlan;
    }
}
