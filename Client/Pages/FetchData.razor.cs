using System.Net.Http.Json;
using BlazorApp.Shared;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Pages
{
    public partial class FetchData
    {
        [Inject]
        private IToastService? ToastService { get; set; }
        private WeatherForecast[] forecasts = new WeatherForecast[]
        {
        };
        private bool IsBusy { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("/api/WeatherForecast") ?? new WeatherForecast[]
                {
                };
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}