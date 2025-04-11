namespace Orientis.Strong.Web.Models;

/// <summary>
/// Represents a single exercise performed in a workout. An exercise can have multiple sets.
/// </summary>
/// <param name="name">The name of the exercise like "Lateral Raise (Dumbbell)".</param>
/// <param name="date">The date the exercise was performed.</param>
public class Exercise(string name, DateTime date)
{
    /// <summary>
    /// The name of the exercise like "Lateral Raise (Dumbbell)".
    /// </summary>
    public string Name { get; } = name;
    /// <summary>
    /// The date the exercise was performed.
    /// </summary>
    public DateTime Date { get; set; } = date;

    /// <summary>
    /// A list of <see cref="Set"/>.
    /// </summary>
    public IReadOnlyCollection<Set> Sets => sets.AsReadOnly();

    /// <summary>
    /// The total volume of the exercise, calculated as the sum of the volume of each set.
    /// </summary>
    public decimal? TotalVolume => sets.Sum(s => s.Volume);
    /// <summary>
    /// The highest volume of the exercise, this is volume of the set with the highest volume.
    /// </summary>
    public decimal? WeightMax => sets.Max(s => s.Volume);
    /// <summary>
    /// The 1RM of the exercise, this is the 1RM of the set with the highest 1RM.
    /// </summary>
    public int? OneRepMax => sets.Max(s => s.OneRepMax);
    /// <summary>
    /// The total number of reps of all sets.
    /// </summary>
    public int? TotalReps => sets.Sum(s => s.Reps);
    /// <summary>
    /// The maximum number of reps in a set.
    /// </summary>
    public int? MaxReps => sets.Max(s => s.Reps);

    /// <summary>
    /// The total number of seconds of all sets.
    /// </summary>
    public int? TotalSeconds => sets.Sum(s => s.Seconds);
    /// <summary>
    /// The maximum number of seconds in a set.
    /// </summary>
    public int? MaxSeconds => sets.Max(s => s.Seconds);

    private readonly IList<Set> sets = [];

    /// <summary>
    /// Adds a set to the exercise.
    /// </summary>
    /// <param name="set">An instance of <see cref="Set"/>.</param>
    public void AddSet(Set set)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        sets.Add(set);
    }
}
