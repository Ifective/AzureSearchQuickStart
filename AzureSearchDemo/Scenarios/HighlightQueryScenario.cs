using System;
using System.Collections.Generic;
using System.Linq;
using AzureSearchDemo.Models;
using Fclp.Internals.Extensions;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class HighlightQueryScenario : BaseSearchScenario
    {
        protected override DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client)
        {
            var parameters = GetDefaultSearchParameters();
            parameters.HighlightFields = new List<string>() { "description", "description_nl" };
            parameters.HighlightPreTag = "<HIGHLIGHT>";
            parameters.HighlightPostTag = "</HIGHLIGHT>";
            return client.Documents.Search<RealEstate>("beautiful", parameters);
        }

        protected override void WriteResult(SearchResult<RealEstate> result)
        {
            Console.WriteLine(result.Document);
            Console.WriteLine("Highlights:");
            result.Highlights.ForEach(h => Console.WriteLine($"-{string.Join("\r\n-", h.Value)}"));
            
            Console.WriteLine();

        }
    }
}