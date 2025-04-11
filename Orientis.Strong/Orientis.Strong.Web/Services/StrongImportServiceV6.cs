using System.Globalization;
using System.Text.RegularExpressions;
using Orientis.Strong.Web.Models;

namespace Orientis.Strong.Web.Services;

public interface IStrongImportService
{
    /// <summary>
    /// Imports a Strong .csv export file and returns a list of <see cref="StrongDataItem"/> for each line in the file.
    /// </summary>
    /// <param name="stream">A <see cref="Stream"/> containing the .csv file data from Strong.</param>
    /// <returns>An list of <see cref="StrongDataItem"/>, each item represents a single line in the Strong export file.</returns>
    Task<IEnumerable<StrongDataItem>> ImportStrongFileAsync(Stream stream);
}

public class StrongImportServiceV6 : IStrongImportService
{
    public async Task<IEnumerable<StrongDataItem>> ImportStrongFileAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var csvData = await reader.ReadToEndAsync();
        var strongData = new List<StrongDataItem>();
        var headers = new Dictionary<string, int>();
        using (var csv = new StringReader(csvData))
        {
            string? line;
            bool isFirstLine = true;
            while ((line = csv.ReadLine()) != null)
            {
                var values = line.Split(';');
                if (isFirstLine)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        var header = values[i].TrimStart('"').TrimEnd('"');
                        headers[header] = i;
                    }
                    isFirstLine = false;
                    continue; // Skip header line
                }
                // Strong V5 format
                //var item = new StrongDataItem
                //{
                //    Date = GetCsvValue<DateTimeOffset>(headers, values, "Date"),
                //    WorkoutName = GetCsvValue<string>(headers, values, "Workout Name") ?? throw new InvalidOperationException("Workout Name not set"),
                //    ExerciseName = GetCsvValue<string>(headers, values, "Exercise Name") ?? throw new InvalidOperationException("Exercise Name not set"),
                //    SetOrder = GetCsvValue<int>(headers, values, "Set Order"),
                //    Weight = GetCsvValue<decimal?>(headers, values, "Weight"),
                //    WeightUnit = GetCsvValue<string>(headers, values, "Weight Unit"),
                //    Seconds = GetCsvValue<int?>(headers, values, "Seconds"),
                //    Reps = GetCsvValue<int?>(headers, values, "Reps"),
                //    WorkoutDuration = GetCsvValue<TimeSpan>(headers, values, "Workout Duration"),
                //};

                // Strong V6 format
                var item = new StrongDataItem
                {
                    Date = GetCsvValue<DateTime>(headers, values, "Date"),
                    WorkoutName = GetCsvValue<string>(headers, values, "Workout Name") ?? throw new InvalidOperationException("Workout Name not set."),
                    ExerciseName = GetCsvValue<string>(headers, values, "Exercise Name") ?? throw new InvalidOperationException("Exercise Name not set."),
                    SetOrder = GetCsvValue<string>(headers, values, "Set Order") ?? throw new InvalidOperationException("Set Order not set."),
                    Weight = GetCsvValue<decimal?>(headers, values, "Weight (kg)"),
                    Seconds = GetCsvValue<decimal?>(headers, values, "Seconds"),
                    Reps = GetCsvValue<int?>(headers, values, "Reps"),
                    WorkoutDuration = GetCsvValue<int>(headers, values, "Duration (sec)")
                };

                strongData.Add(item);
            }
        }
        return strongData;
    }

    private static T? GetCsvValue<T>(Dictionary<string, int> headers, string[] values, string headerName)
    {
        ArgumentNullException.ThrowIfNull(headers, nameof(headers));
        ArgumentNullException.ThrowIfNull(headerName, nameof(headerName));

        if (!headers.TryGetValue(headerName, out int index))
        {
            throw new InvalidOperationException($"Header '{headerName}' not found in CSV file.");
        }

        var value = values[index]?.TrimStart('"').TrimEnd('"');

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        if (typeof(T) == typeof(DateTime?) || typeof(T) == typeof(DateTime))
        {
            var formatProvider = CultureInfo.InvariantCulture.DateTimeFormat;
            return (T)(object)DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", formatProvider);
        }

        if (typeof(T) == typeof(decimal?) || typeof(T) == typeof(decimal))
        {
            var formatProvider = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            formatProvider!.NumberFormat.NumberDecimalSeparator = ".";
            return (T)(object)decimal.Parse(value, formatProvider);
        }

        if (typeof(T) == typeof(TimeSpan?) || typeof(T) == typeof(TimeSpan))
        {
            return (T)(object)ParseDuration(value);
        }

        if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
        {
            return (T)(object)int.Parse(value);
        }

        var type = typeof(T).Name;

        return (T)Convert.ChangeType(value, typeof(T));
    }

    private static TimeSpan ParseDuration(string input)
    {
        // Regular expression to extract hours and minutes (optional h and m)
        var match = Regex.Match(input, @"(?:(\d+)h)?(?:(\d+)m)?");

        if (!match.Success)
        {
            throw new FormatException("Invalid duration format.");
        }

        int hours = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
        int minutes = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return new TimeSpan(hours, minutes, 0);
    }
}
