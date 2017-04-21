using System;
using System.Collections.Generic;
using System.Linq;
using Fclp.Internals.Extensions;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public class StandardSuggestScenario : BaseSuggestScenario
    {
        protected override void PerformSuggest(ISearchIndexClient client)
        {
            var suggestionResult = client.Documents.Suggest("b", "sg", new SuggestParameters()
            {
                SearchFields = new List<string>() {"city"},
                UseFuzzyMatching = true
            });

            suggestionResult.Results.GroupBy(r => r.Text).ForEach(g => Console.WriteLine(g.Key));
            Console.WriteLine();
            
        }
    }
}