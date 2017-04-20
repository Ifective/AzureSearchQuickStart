using System.Collections.Generic;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class AdvancedQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            return client.Documents.Search<RealEstate>("(great views) | (beautiful home)", GetDefaultSearchParameters());
        }
    }
}