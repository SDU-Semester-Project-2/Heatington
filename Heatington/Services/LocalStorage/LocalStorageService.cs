using Blazored.LocalStorage;
using Heatington.Models;

namespace Heatington.Services.LocalStorage;
// TODO: May implement this later if everything is working and I have time to refactor. This would be a SOLID solution,
// but it will increase the layer and I currently mainly focus on functional application.
public class LocalStorageService(ILocalStorageService localStorage)
{
    private readonly ILocalStorageService _localStorage = localStorage;

    public async Task SaveProductionUnitToLocalStorage(ProductionUnit productionUnit)
    {
        await _localStorage.SetItemAsync("productionUnit", productionUnit).ConfigureAwait(false);
    }

    public async Task<ProductionUnit?> RetrieveProductionUnitFromLocalStorage()
    {
        return await _localStorage.GetItemAsync<ProductionUnit>("productionUnit").ConfigureAwait(false);
    }
}
