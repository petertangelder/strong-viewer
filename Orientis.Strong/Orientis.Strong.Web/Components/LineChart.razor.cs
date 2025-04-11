using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Orientis.Strong.Web.Components;

public partial class LineChart : IAsyncDisposable
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public object? Data { get; set; }

    [Parameter]
    public bool ShowRightYAxis { get; set; } = true;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    // This is used to detect if the data has changed
    private string? _previousDataHash;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Data != null)
        {
            var config = new
            {
                Type = "line",
                Options = new
                {
                    Responsive = true,
                    MaintainAspectRatio = false,
                    Scales = new
                    {
                        x = new
                        {
                            Type = "time",
                            Time = new
                            {
                                DisplayFormats = new
                                {
                                    Day = "dd MMM"
                                }
                            },
                            Ticks = new
                            {
                                MaxTicksLimit = 10
                            }
                        },
                        yl = new
                        {
                            Position = "left",
                            Display = true,
                            Ticks = new
                            {
                                // Will be replaced by an actual JavaScript function in the chart.js file
                                Callback = "window.chartHelper.formatInteger"
                            },
                        },
                        yr = new
                        {
                            Position = "right",
                            Display = ShowRightYAxis,
                            Grid = new
                            {
                                DrawOnChartArea = false,
                            }
                        },
                        ylh = new
                        {
                            Position = "left",
                            Display = false,
                            Grid = new
                            {
                                DrawOnChartArea = false,
                            }
                        }
                    },
                    Interaction = new
                    {
                        Mode = "index",
                        Intersect = true
                    },
                    Plugins = new
                    {
                        Tooltip = new
                        {
                            Callbacks = new
                            {
                                Title = "window.chartHelper.formatTooltipTitle",
                                Label = "window.chartHelper.formatTooltipLabel",
                                Footer = "window.chartHelper.formatTooltipFooter"
                            }
                        }
                    }

                },
                Data = Data
            };

            var data = (dynamic)Data;
            var exerciseType = data.Datasets[0].Data[0].sets[0].ExerciseType;
            await JSRuntime.InvokeVoidAsync("setup", Id, config, (int)exerciseType);
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        var json = JsonSerializer.Serialize(Data);
        var currentHash = ComputeSha256Hash(json);

        if (_previousDataHash != currentHash)
        {
            _previousDataHash = currentHash;

            await JSRuntime.InvokeVoidAsync("update", Id, Data);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (JSRuntime is not null)
        {
            await JSRuntime.InvokeVoidAsync("dispose", Id);
        }
    }

    private static string ComputeSha256Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
