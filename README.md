# Orientis Strong viewer
Tracking progress is an important part of any gym routine. The Strong app is great for logging workout sets, but its built-in progress charts can be hard to read. With this web app, you can export your workout data from Strong and import it here to get clearer, more insightful visualizations of your progress.

# Blazor WebAssembly App

This web application is built using **.NET 9** and the **Blazor WebAssembly** hosting model. It runs entirely client-side in the browser, with all dependent packages included directly in the app rather than referenced separately. The Chart.js library is used to display data in line charts.

## Features

- **Blazor WebAssembly**: Fully client-side application using modern .NET.
- **Chart.js Integration**: View line charts of your exercises over time with Chart.js.
- **Radzen Blazor Components**: Free UI components for Blazor apps.
- **Self-contained**: All dependencies are bundled with the app, no server-side references required.
- **Visual Studio Ready**: Easily build and run the app using Visual Studio.

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with Blazor development support

### Clone the repository

```bash
git clone https://github.com/petertangelder/strong-viewer.git
cd strong-viewer
```

### Run the app

1. Open the solution in Visual Studio.
2. Set the project as the startup project (if not already).
3. Click **Run** or press `F5`.

The app should launch in your browser and run entirely client-side.

## Usage

Upload the exported workout data from the Strong app (.csv file) and select a workout template. Line charts powered by Chart.js will be used to display your data interactively.

## Technologies used

- [.NET 9](https://dotnet.microsoft.com/)
- [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [Radzen Blazor Components](https://blazor.radzen.com)
- [Chart.js](https://www.chartjs.org/)

## Strong links

- [Strong app](https://www.strong.app/)
- [Strong data export](https://help.strongapp.io/article/235-export-workout-data)

## License

The Orientis Strong viewer is licensed under the [GNU General Public License v3.0 license](LICENSE).
