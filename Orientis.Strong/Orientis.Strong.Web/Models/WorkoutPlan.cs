namespace Orientis.Strong.Web.Models;

/// <summary>
/// A workout plan is a collection of workouts performed over a period of time. The plan
/// is a template of exercises, the workouts are the actual training sessions performed.
/// If a split workout routine is used, like "Push/Pull/Legs" or "Upper/Lower", it will
/// result in 3 cq 2 workout plans.
/// </summary>
/// <param name="name">The name of the workout plan like "Push" or "Fullbody A".</param>
/// <param name="from">The date of the first workout performed in this plan.</param>
/// <param name="to">The date of the last workout performed in this plan.</param>
public class WorkoutPlan(string name, DateTime from, DateTime to)
{
    /// <summary>
    /// The name of the workout plan like "Push" or "Fullbody A".
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// The display name of the plan, it contains the name and the date range of the plan.
    /// </summary>
    public string DisplayName => $"{Name} ({From:MMM yy}-{To:MMM yy})";

    /// <summary>
    /// The date of the first workout performed in this plan.
    /// </summary>
    public DateTime From { get; set; } = from;

    /// <summary>
    /// The date of the last workout performed in this plan. This date is updated every time
    /// a new workout is added to the plan.
    /// </summary>
    public DateTime To { get; set; } = to;

    /// <summary>
    /// All workouts performed in this plan. The workouts are the actual training sessions.
    /// </summary>
    public IReadOnlyCollection<Workout> Workouts => workouts.AsReadOnly();

    private readonly IList<Workout> workouts = [];

    /// <summary>
    /// Adds a workout to the workout plan, the workout is an actual training session.
    /// </summary>
    /// <param name="workout"></param>
    public void AddWorkout(Workout workout)
    {
        ArgumentNullException.ThrowIfNull(workout, nameof(workout));

        workouts.Add(workout);

        // A workout was performed, so the date end date of this plan is updated accordingly.
        if (workout.Date > To)
        {
            To = workout.Date;
        }
    }
}
