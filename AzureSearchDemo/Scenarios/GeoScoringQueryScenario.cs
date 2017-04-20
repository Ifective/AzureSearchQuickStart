using System.Collections.Generic;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;

namespace AzureSearchDemo.Scenarios
{
    public class GeoScoringQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.ScoringProfile = "BoostNearbyLocation";
            parameters.ScoringParameters = new List<ScoringParameter>()
            {
                new ScoringParameter("currentLocation", GeographyPoint.Create(47.5679, -122.29))
            };
            return client.Documents.Search<RealEstate>("beautiful", parameters);
        }
    }
}