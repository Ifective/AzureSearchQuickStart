using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class StandardQueryWithFilterScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.Filter = "beds ge 2 and sqft gt 16000";
            return client.Documents.Search<RealEstate>("Bellevue", parameters);
        }
    }
}