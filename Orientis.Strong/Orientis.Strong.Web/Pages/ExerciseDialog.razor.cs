using Microsoft.AspNetCore.Components;
using Orientis.Strong.Web.Models;
using Radzen;

namespace Orientis.Strong.Web.Pages;

public partial class ExerciseDialog
{
    [Parameter]
    public IEnumerable<ExerciseChartModel>? Exercises { get; set; }

    private IEnumerable<ExerciseChartModel>? FilteredExercises { get; set; }
    private DateTime? MinDate => Exercises?.Min(x => x.Date);
    private DateTime? MaxDate => Exercises?.Max(x => x.Date).AddDays(-7);
    private DateTime? StartDate { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            FilteredExercises = Exercises;
            StartDate = MinDate;
            StateHasChanged();
        }
    }

    private void DateChanged(DateTime? dateTime)
    {
        if (Exercises == null)
        {
            return;
        }
        
        if (dateTime.HasValue)
        {
            FilteredExercises = [.. Exercises.Where(x => x.Date.Date >= dateTime.Value.Date)];
        }
        else
        {
            FilteredExercises = Exercises;
        }
    }

    private void DateRender(DateRenderEventArgs args)
    {
        if (Exercises == null)
        {
            return;
        }
        var exerciseOnThisDate = Exercises.Select(d => d.Date.Date).Contains(args.Date.Date);
        if (exerciseOnThisDate)
        {
            args.Attributes.Add("style", "background-color: #d7bbe4; border-color: #d7bbe4;");
        }
    }
}
