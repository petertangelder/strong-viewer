using Microsoft.AspNetCore.Components;
using Orientis.Strong.Web.Models;
using Orientis.Strong.Web.Services;
using Radzen;

namespace Orientis.Strong.Web.Pages;

public partial class Home
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;

    [Inject]
    private IStrongImportService StrongImportService { get; set; } = default!;

    [Inject]
    private ILogger<Home> Logger { get; set; } = default!;

    private bool DataLoaded => WorkoutPlanNames.Count() > 0;
    private StrongData? StrongData;
    private IEnumerable<Workout> Workouts = [];
    private IEnumerable<string> WorkoutPlanNames = [];
    private string SelectedWorkoutPlan = string.Empty;
    private IDictionary<string, IEnumerable<ExerciseChartModel>>? Exercises;

    private bool IsUploading;

    private string? ErrorMessage;

    private async void OnExportFileUpload(UploadChangeEventArgs args)
    {
        IsUploading = true;
        StateHasChanged();
        try
        {
            if (args.Files?.Count() > 0)
            {
                WorkoutPlanNames = [];
                Exercises = null;
                SelectedWorkoutPlan = "";
                WorkoutPlanNames = [];

                StateHasChanged();

                var file = args.Files.First();
                using var stream = file.OpenReadStream();
                var strongDataItems = await StrongImportService.ImportStrongFileAsync(stream);
                StrongData = new StrongData();
                StrongData.AddAllWorkouts(strongDataItems);

                if (StrongData.WorkoutPlans.Count == 0)
                {
                    return;
                }

                WorkoutPlanNames = StrongData.WorkoutPlans.OrderByDescending(x => x.From).Select(x => x.DisplayName).Distinct();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Error processing file: {Message}", ex.Message);
            WorkoutPlanNames = [];
            ErrorMessage = "Could not process the uploaded file.";
        }
        finally
        {
            IsUploading = false;
        }
        StateHasChanged();
    }

    private void WorkoutPlanSelected(object workout)
    {
        try
        {
            if (workout == null)
            {
                return;
            }
            Exercises = null;
            StateHasChanged();

            SelectedWorkoutPlan = workout.ToString() ?? string.Empty;

            var selectedWorkoutPlan = StrongData!.WorkoutPlans.Single(x => x.DisplayName == SelectedWorkoutPlan);
            var exercisesInPlan = StrongData.GetExercisesFromWorkoutPlan(selectedWorkoutPlan);

            // Group the exercises by name and create a dictionary of ExerciseChartModel for each exercise
            // This results in 1 chart per exercise
            var exercisesGroupByName = exercisesInPlan.GroupBy(x => x.Name);
            Exercises = exercisesGroupByName.ToDictionary(x => x.Key, x => ExerciseChartModel.FromExercises([.. x]));
        }
        catch (Exception ex)
        {
            Logger.LogError("Error processing data from the imported workout file: {Message}", ex.Message);
            WorkoutPlanNames = [];
            ErrorMessage = "File was imported successfully, but could not process the data.";
            StateHasChanged();
        }
    }

    private async Task ShowExerciseDialogAsync(string exerciseName)
    {
        var exercises = StrongData!.GetAllExercisesByName(exerciseName);
        var exerciseChartModels = ExerciseChartModel.FromExercises(exercises);

        await DialogService.OpenAsync<ExerciseDialog>(exerciseName,
            new Dictionary<string, object>() { { "Exercises", exerciseChartModels } }, new DialogOptions() { Width = "1200px" });
    }

    private string GetChartId(string exerciseName)
    {
        return $"chart-{exerciseName.ToLowerInvariant()}".Replace(" ", "-").Replace("(", "").Replace(")", "");
    }
}
