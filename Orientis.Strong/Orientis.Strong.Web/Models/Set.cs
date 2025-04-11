using Orientis.Strong.Web.Enums;

namespace Orientis.Strong.Web.Models;

/// <summary>
/// Represents a single set of an exercise performed in a workout. It can be a set of a weight lifting exercise,
/// with a weight and a number of reps, a set of a body weight exercise, with a number of reps without weight,
/// or a set of a certain duration, like a "Plank".
/// </summary>
/// <param name="order">The number of the set within the workout.</param>
/// <param name="weight">The used weight, can be 0 for a body weight exercise.</param>
/// <param name="weightUnit">The unit of the weight (e.g. "kg").</param>
/// <param name="reps">The number of reps performed in the set. Can be 0 for duration exercises like "Plank".</param>
/// <param name="seconds">The duration of the set in seconds, for exercises like "Plank".</param>
public class Set(int order, decimal? weight, string? weightUnit, int? reps, int? seconds)
{
    /// <summary>
    /// The number of the set within the workout. This is a number starting from 1.
    /// </summary>
    public int Order { get; } = order;
    /// <summary>
    /// The weight used in the set. Weight can be 0 for e.g. body weight exercises.
    /// </summary>
    public decimal? Weight { get; } = weight;
    /// <summary>
    /// The unit of the weight (e.g. "kg").
    /// </summary>
    public string? WeightUnit { get; } = weightUnit;
    /// <summary>
    /// The number of reps performed in the set. Can be 0 for duration exercises like "Plank".
    /// </summary>
    public int? Reps { get; } = reps;
    /// <summary>
    /// The duration of the set in seconds, for exercises like "Plank".
    /// </summary>
    public int? Seconds { get; } = seconds;

    /// <summary>
    /// The total volume of the set, calculated as weight * reps. This is only valid for weight lifting exercises.
    /// </summary>
    public decimal? Volume => Weight.HasValue && Weight.Value > 0 && Reps.HasValue ? Weight.Value * Reps.Value : null;
    /// <summary>
    /// The one rep max of the set, calculated using the Brzycki formula. This is only valid for weight lifting exercises.
    /// One rep maximum (or 1RM) is an estimate of your best possible single rep lift for an exercise. 
    /// </summary>
    public int? OneRepMax => Weight.HasValue && Weight.Value > 0 && Reps.HasValue ? Calculate1RM(Weight.Value, Reps.Value) : null;

    /// <summary>
    /// The type of the exercise. When weight and reps are set, it is a weight lifting exercise. If only reps are set, it
    /// is a body weight exercise. If only seconds are set, it is a duration exercise.
    /// </summary>
    public ExerciseType ExerciseType => Weight.HasValue && Weight.Value > 0 && Reps.HasValue ? ExerciseType.WeightLifting :
                                        Reps.HasValue ? ExerciseType.BodyWeight :
                                        Seconds.HasValue ? ExerciseType.Duration :
                                        throw new InvalidOperationException("Unkown exercise type.");

    /// <summary>
    /// Calculates the one rep max using the Brzycki formula. This is only valid for weight lifting exercises.
    /// One rep maximum (or 1RM) is an estimate of your best possible single rep lift for an exercise. 
    /// </summary>
    /// <param name="weight">The used weight in the set.</param>
    /// <param name="reps">The number of reps in the set.</param>
    /// <returns>An estimate of the best possible single rep.</returns>
    /// <exception cref="ArgumentException"></exception>
    private static int Calculate1RM(decimal weight, int reps)
    {
        if (reps <= 0)
        {
            throw new ArgumentException("Reps must be greater than zero.");
        }

        // Calculate one rep max using the Brzycki formula
        return (int)Math.Round(weight * (36 / (37 - (decimal)reps)), MidpointRounding.AwayFromZero);
    }
}
