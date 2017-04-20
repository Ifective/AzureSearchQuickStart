using System.Collections.Generic;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class StandardQuerySpecificLanguageScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.SearchFields = new List<string>() {"description_nl"};
            return client.Documents.Search<RealEstate>("lopen & -loopafstand", parameters);
        }
    }
}