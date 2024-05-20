using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Heatington.AssetManager;
using Heatington.Models;
using Heatington.Web.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Heatington.Web.Client.Pages
{
    public partial class ResourceManager : ComponentBase
    {
        private bool _isLoading = false;


        List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
        private string _selectedSeason = "Winter";
        private List<HeatDemandData> heatDemandDataList = new List<HeatDemandData>();

        public string SelectedSeason
        {
            get { return _selectedSeason; }
            set
            {
                if (_selectedSeason != value)
                {
                    _selectedSeason = value;
                    _ = LoadDataForSeason();
                }
            }
        }

        [Inject] public required IDialogService DialogService { get; set; }
        [Inject] public required HttpClient Http { get; set; }

        [Inject] public required ILogger<ResourceManager> Logger { get; set; }
        //TODO: Take a look at this, should it be like this or just list because of GUID
        // public Dictionary<ProductionUnitsEnum, ProductionUnit> _productionUnits;

        [Obsolete]
        async void OpenDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("ProductionUnits", _productionUnits);

            var dialog = DialogService.Show<AddBoilerDialog>("Add Production Unit", parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }

        [Obsolete]
        async void OpenDialog(ProductionUnit productionUnit)
        {
            var parameters = new DialogParameters();
            parameters.Add("ProductionUnit", productionUnit); // Pass selected unit

            var dialog = DialogService.Show<EditBoilerDialog>(($"Edit {productionUnit.FullName}"), parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }

        private async Task LoadDataForSeason()
        {
            string apiEndpoint = $"http://localhost:5165/api/TimeSeriesData?season={_selectedSeason.ToLower()}";
            string jsonContent = await Http.GetStringAsync(apiEndpoint);
            Logger.LogInformation("Fetched JSON content: " + jsonContent.Substring(0, 200));
            heatDemandDataList.Clear();
            ParseJsonData(jsonContent);
            StateHasChanged();
        }

        private string DisplayData(double data) =>
            Convert.ToString(data.ToString().Length != 0 ? data.ToString().Length : "No Data");

        private async Task ReadCsvDataOnLoad(string pathToFile)
        {
            Console.WriteLine(pathToFile);
            string csvContent = await File.ReadAllTextAsync(pathToFile);

            ParseCsvData(csvContent);
        }

        private async Task HandleCSVUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;

            var reader = new StreamReader(file.OpenReadStream());
            var csvContent = await reader.ReadToEndAsync();

            ParseCsvData(csvContent);
        }

        private void ParseCsvData(string csvContent)
        {
            var rows = csvContent.Split('\n');

            // Use CultureInfo to handle potential localization issues with decimal point/comma
            var culture = CultureInfo.InvariantCulture;

            foreach (var row in rows)
            {
                var values = row.Split(',');
                if (values.Length == 4)
                {
                    heatDemandDataList.Add(new HeatDemandData
                    {
                        StartDate = DateTime.Parse(values[0], culture),
                        EndDate = DateTime.Parse(values[1], culture),
                        HeatDemand = float.Parse(values[2], culture),
                        ElectricityPrice = float.Parse(values[3], culture),
                    });
                }
            }

            StateHasChanged();
        }

        private void ParseJsonData(string jsonContent)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new DateTimeConverter() }
            };

            var data = JsonSerializer.Deserialize<List<HeatDemandData>>(jsonContent, options);

            foreach (var item in data)
            {
                heatDemandDataList.Add(item);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Logger.LogInformation("OnInitializedAsync started.");

                await base.OnInitializedAsync();

                ProductionUnit[]? productionUnitsArray =
                    await Http.GetFromJsonAsync<ProductionUnit[]>("http://localhost:5271/api/productionunits");

                if (productionUnitsArray != null)
                {
                    _productionUnits = productionUnitsArray.ToList();

                    foreach (ProductionUnit unit in _productionUnits)
                    {
                        try
                        {
                            string url =
                                $"http://localhost:5271/api/Images/{unit.PicturePath.Replace("AssetManager/", "")}";
                            string base64ImageData = await Http.GetStringAsync(url);
                            unit.PictureBase64Url = $"data:image/jpg;base64, {base64ImageData}";
                        }
                        catch
                        {
                            Logger.LogError($"Error occurered while fetching image {unit.PicturePath}");
                            throw new Exception();
                        }
                    }
                }

                if (heatDemandDataList.Count == 0)
                {
                    string jsonContent =
                        await Http.GetStringAsync("http://localhost:5165/api/TimeSeriesData?season=winter");
                    Logger.LogInformation("Fetched JSON content: " + jsonContent.Substring(0, 200));
                    ParseJsonData(jsonContent);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred in OnInitializedAsync.");
            }
        }

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), format: "yyyy-MM-ddTHH:mm:ss",
                    CultureInfo.InvariantCulture);
            }

            public override void Write(Utf8JsonWriter writer, DateTime dateTimeValue, JsonSerializerOptions options)
            {
                writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
        }

        public class HeatDemandData
        {
            [JsonPropertyName("startTime")] public DateTime StartDate { get; set; }

            [JsonPropertyName("endTime")] public DateTime EndDate { get; set; }

            [JsonPropertyName("heatDemand")] public float HeatDemand { get; set; }

            [JsonPropertyName("electricityPrice")] public float ElectricityPrice { get; set; }
        }
    }
}
