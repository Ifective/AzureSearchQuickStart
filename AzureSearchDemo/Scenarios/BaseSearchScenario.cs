using System;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public abstract class BaseSearchScenario : IBaseScenario
    {
        private string _searchServiceName;
        private string _adminApiKey;
        private string _indexName;
        private bool _isInitialized;

        public void Init(string searchServiceName, string adminApiKey, string indexName)
        {
            _searchServiceName = searchServiceName;
            _adminApiKey = adminApiKey;
            _indexName = indexName;
            _isInitialized = true;
        }

        public void Start()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Not initialized!");

            var serviceClient = CreateSearchServiceClient();
            var indexClient = serviceClient.Indexes.GetClient(_indexName);
            
            var results = PerformSearch(indexClient);
            WriteDocuments(results);
        }

        protected abstract DocumentSearchResult<RealEstate> PerformSearch(ISearchIndexClient client);

        protected virtual void WriteDocuments(DocumentSearchResult<RealEstate> searchResults)
        {
            foreach (SearchResult<RealEstate> result in searchResults.Results)
            {
                WriteResult(result);
            }
            Console.WriteLine();
            Console.WriteLine($"Total results: {searchResults.Count}");
            Console.WriteLine();
        }

        protected virtual void WriteResult(SearchResult<RealEstate> result)
        {
            Console.WriteLine(result.Document);
            Console.WriteLine();
        }

        protected SearchParameters GetDefaultSearchParameters()
        {
            return new SearchParameters()
            {
                IncludeTotalResultCount = true,
                Select = new[] { "listingId", "description", "description_nl", "beds", "city", "sqft" }
            };
        }

        protected virtual SearchServiceClient CreateSearchServiceClient()
        {
            var serviceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));
            return serviceClient;
        }
    }
}