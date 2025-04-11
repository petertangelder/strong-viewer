namespace Orientis.Strong.Web.Models;

/// <summary>
/// A workout is a collection of exercises performed in a single training session. A workout is part of a workout plan.
/// </summary>
/// <param name="date">The date of the training session.</param>
/// <param name="duration">The total training time.</param>
public class Workout(DateTime date, TimeSpan duration)
{
    /// <summary>
    /// The date the workout was performed.
    /// </summary>
    public DateTime Date { get; set; } = date;
    /// <summary>
    /// The total duration of the workout.
    /// </summary>
    public TimeSpan Duration { get; set; } = duration;

    /// <summary>
    /// A list of <see cref="Exercise"/>. Each exercise can have multiple sets.
    /// </summary>
    public IReadOnlyCollection<Exercise> Exercises => _exercises.AsReadOnly();

    private readonly IList<Exercise> _exercises = [];

    /// <summary>
    /// Adds a set of an exercise to the workout. If the exercise already exists in the workout, a new set will be added to it.
    /// Otherwise, the exercise will be addd to the workout with the set.
    /// </summary>
    /// <param name="exerciseName">The name of the exercise like "Bench Press (Barbell)".</param>
    /// <param name="setDate">The date this set was performed.</param>
    /// <param name="set">Details of the set in a <see cref="Set"/> instance.</param>
    public void AddSet(string exerciseName, DateTime setDate, Set set)
    {
        ArgumentException.ThrowIfNullOrEmpty(exerciseName, nameof(exerciseName));
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        var exercise = _exercises.SingleOrDefault(x => x.Name == exerciseName && x.Date == setDate);
        if (exercise == null)
        {
            exercise = new Exercise(exerciseName, setDate);
            _exercises.Add(exercise);
        }

        exercise.AddSet(set);
    }
}
