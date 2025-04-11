namespace Orientis.Strong.Web.Models;

/// <summary>
/// Represents a single line in the Strong export file.
/// </summary>
public class StrongDataItem
{
    /// <summary>
    /// The date the set was performed. All items with the same date are part of the same workout.
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// The name of the workout template like "Fullbody A" or "Push". All workouts with the same
    /// name are part of the same workout plan (unless there is a gap of two months or more).
    /// </summary>
    public string WorkoutName { get; set; } = null!;
    /// <summary>
    /// The name of the exercise like "Bench Press (Barbell)" or "Chest Fly (Dumbbell)".
    /// </summary>
    public string ExerciseName { get; set; } = null!;
    /// <summary>
    /// The order of the set in the workout. This is a number starting from 1, can also
    /// contain values like "Note" or "Rest Timer". If the value is not a number, it is ignored.
    /// </summary>
    public string SetOrder { get; set; } = null!;
    /// <summary>
    /// The weight used in the set. This app assumes the weight is in kg. Weight can be 0
    /// for e.g. body weight exercises.
    /// </summary>
    public decimal? Weight { get; set; }
    /// <summary>
    /// The duration of the set in seconds, for exercises like "Plank".
    /// </summary>
    public decimal? Seconds { get; set; }
    /// <summary>
    /// The number of reps performed in the set. Can be 0 for duration exercises like "Plank".
    /// </summary>
    public int? Reps { get; set; }
    /// <summary>
    /// The total duration of the workout in seconds.
    /// </summary>
    public int WorkoutDuration { get; set; }
}
