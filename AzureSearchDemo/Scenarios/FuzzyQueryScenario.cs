using System.Collections.Generic;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class FuzzyQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.QueryType = QueryType.Full;
            return client.Documents.Search<RealEstate>("beautfl~ & ocean" , parameters);
        }
    }
}