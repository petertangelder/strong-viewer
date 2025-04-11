using Microsoft.AspNetCore.Components;
using Orientis.Strong.Web.Enums;
using Orientis.Strong.Web.Models;

namespace Orientis.Strong.Web.Components;

public partial class ExerciseChart
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public IEnumerable<ExerciseChartModel>? Exercises { get; set; }

    private ExerciseType? ChartExerciseType => Exercises != null && Exercises.Any() ? Exercises.First().ExerciseType : null;

    private object TotalVolumeData => GetTotalVolumeDataSets();
    private object TotalRepsData => GetTotalRepsDataSet();
    private object TotalSecondsData => GetTotalSecondsDataSet();

    private const int BorderWidthValue = 2;
    private const int PointRadiusValue = 5;
    private const int PointHoverRadiusValue = 7;

    private object GetTotalVolumeDataSets()
    {
        return new
        {
            Datasets = new[]
            {
                new
                {
                    Label = "Total volume",
                    YAxisID = "yl",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.TotalVolume, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#791da4",
                    BorderColor = "#791da4",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#791da4",
                    PointBorderColor = "#791da4"
                },
                new
                {
                    Label = "Best set (1 RM)",
                    YAxisID = "yr",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.OneRepMax, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#f62ddf",
                    BorderColor = "#f62ddf",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#f62ddf",
                    PointBorderColor = "#f62ddf",
                },
                new
                {
                    Label = "Best set (max weight)",
                    YAxisID = "ylh",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.WeightMax, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#757575",
                    BorderColor = "#757575",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#757575",
                    PointBorderColor = "#757575",
                }
            }
        };
    }

    private object GetTotalRepsDataSet()
    {
        return new
        {
            Datasets = new[]
            {
                new
                {
                    Label = "Total reps",
                    YAxisID = "yl",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.TotalReps, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#791da4",
                    BorderColor = "#791da4",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#791da4",
                    PointBorderColor = "#791da4"
                },
                new
                {
                    Label = "Best set (reps)",
                    YAxisID = "ylh",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.MaxReps, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#757575",
                    BorderColor = "#757575",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#757575",
                    PointBorderColor = "#757575",
                }
            }
        };
    }

    private object GetTotalSecondsDataSet()
    {
        return new
        {
            Datasets = new[]
            {
                new
                {
                    Label = "Total time",
                    YAxisID = "yl",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.TotalSeconds, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#791da4",
                    BorderColor = "#791da4",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#791da4",
                    PointBorderColor = "#791da4"
                },
                new
                {
                    Label = "Best set (time)",
                    YAxisID = "ylh",
                    Data = Exercises?.Select(x => new { x = x.Date, y = x.MaxSeconds, sets = x.Sets }).ToArray(),
                    BorderWidth = BorderWidthValue,
                    BackgroundColor = "#757575",
                    BorderColor = "#757575",
                    PointStyle = "rect",
                    PointRadius = PointRadiusValue,
                    PointHoverRadius = PointHoverRadiusValue,
                    PointBackgroundColor = "#757575",
                    PointBorderColor = "#757575",
                }
            }
        };
    }
}
