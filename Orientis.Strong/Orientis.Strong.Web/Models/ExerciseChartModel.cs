using Orientis.Strong.Web.Enums;

namespace Orientis.Strong.Web.Models;

/// <summary>
/// Represents a single point in an exercise chart, containing data of an exercise performed on a certain day.
/// </summary>
/// <param name="name">The name of the exercise like "Hammer Curl (Dumbbell)".</param>
/// <param name="date">The date the exercise was performed.</param>
/// <param name="exerciseType">The <see cref="ExerciseType"/>, used to determine which lines to draw in the chart.</param>
/// <param name="totalVolume">The total volume of the exercise, calculated as the sum of the volume of each set.</param>
/// <param name="weightMax">The highest volume of the exercise, this is volume of the set with the highest volume.</param>
/// <param name="oneRepMax">The 1RM of the exercise, this is the 1RM of the set with the highest 1RM.</param>
/// <param name="totalReps">The total number of reps of all sets.</param>
/// <param name="maxReps">The maximum number of reps in a set.</param>
/// <param name="totalSeconds">The total number of seconds of all sets.</param>
/// <param name="maxSeconds">The maximum number of seconds in a set.</param>
/// <param name="sets">The sets performed this day.</param>
public class ExerciseChartModel(string name, DateTime date, ExerciseType exerciseType, decimal? totalVolume, decimal? weightMax, int? oneRepMax, int? totalReps, int? maxReps, int? totalSeconds, int? maxSeconds, IEnumerable<Set> sets)
{
    /// <summary>
    /// The name of the exercise like "Lateral Raise (Dumbbell)".
    /// </summary>
    public string Name { get; } = name;
    /// <summary>
    /// The date the exercise was performed.
    /// </summary>
    public DateTime Date { get; } = date;
    /// <summary>
    /// The type of the exercise. When weight and reps are set, it is a weight lifting exercise. If only reps are set, it
    /// is a body weight exercise. If only seconds are set, it is a duration exercise.
    /// </summary>
    public ExerciseType ExerciseType { get; } = exerciseType;
    /// <summary>
    /// The total volume of the exercise, calculated as the sum of the volume of each set.
    /// </summary>
    public int? TotalVolume { get; } = (int?)totalVolume;
    /// <summary>
    /// The highest volume of the exercise, this is volume of the set with the highest volume.
    /// </summary>
    public int? WeightMax { get; } = (int?)weightMax;
    /// <summary>
    /// The 1RM of the exercise, this is the 1RM of the set with the highest 1RM.
    /// </summary>
    public int? OneRepMax { get; } = oneRepMax;

    /// <summary>
    /// The total number of reps of all sets.
    /// </summary>
    public int? TotalReps { get; } = totalReps;
    /// <summary>
    /// The maximum number of reps in a set.
    /// </summary>
    public int? MaxReps { get; } = maxReps;

    /// <summary>
    /// The total number of seconds of all sets.
    /// </summary>
    public int? TotalSeconds { get; } = totalSeconds;
    /// <summary>
    /// The maximum number of seconds in a set.
    /// </summary>
    public int? MaxSeconds { get; } = maxSeconds;

    /// <summary>
    /// The sets performed this day, list of <see cref="Set"/>.
    /// </summary>
    public IReadOnlyCollection<Set> Sets { get; } = sets.ToList().AsReadOnly();

    /// <summary>
    /// Transforms a collection of <see cref="Exercise"/> into a collection of <see cref="ExerciseChartModel"/>.
    /// </summary>
    /// <param name="exercises"></param>
    /// <returns></returns>
    public static IEnumerable<ExerciseChartModel> FromExercises(IEnumerable<Exercise> exercises)
    {
        ArgumentNullException.ThrowIfNull(exercises, nameof(exercises));

        if (!exercises.Any())
        {
            return [];
        }
        return exercises.Select(x => new ExerciseChartModel(x.Name, x.Date, x.Sets.First().ExerciseType, x.TotalVolume, x.WeightMax, x.OneRepMax, x.TotalReps, x.MaxReps, x.TotalSeconds, x.MaxSeconds, x.Sets));
    }
}
