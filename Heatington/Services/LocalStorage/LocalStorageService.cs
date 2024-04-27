using Blazored.LocalStorage;
using Heatington.Models;

namespace Heatington.Services.LocalStorage;

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
