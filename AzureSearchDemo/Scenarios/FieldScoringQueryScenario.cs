using System.Collections.Generic;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class FieldScoringQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.ScoringProfile = "BoostCity";
            return client.Documents.Search<RealEstate>("Belle*", parameters);
        }
    }
}