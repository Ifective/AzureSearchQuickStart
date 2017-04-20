using System;
using System.Collections.Generic;
using System.Linq;
using AzureSearchDemo.Models;
using Fclp.Internals.Extensions;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class FacetQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.Facets = new List<string>() { "sqft,interval:100"};
            return client.Documents.Search<RealEstate>("beautiful", parameters);
        }

        protected override void WriteDocuments(DocumentSearchResult<RealEstate> searchResults)
        {
            base.WriteDocuments(searchResults);
            Console.WriteLine();
            Console.WriteLine($"Facets:");
            searchResults.Facets.ForEach(f => Console.WriteLine($"{f.Key}:\r\n{string.Join("\r\n",f.Value.Select(v => $"Value: {v.Value} Count: {v.Count}"))}"));
            Console.WriteLine();
        }
    }
}