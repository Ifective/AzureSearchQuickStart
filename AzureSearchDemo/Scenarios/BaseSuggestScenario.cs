using System;
using AzureSearchDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchDemo.Scenarios
{
    public abstract class BaseSuggestScenario : IBaseScenario
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
            
            PerformSuggest(indexClient);
        }

        protected abstract void PerformSuggest(ISearchIndexClient client);
        

        protected virtual SearchServiceClient CreateSearchServiceClient()
        {
            var serviceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));
            return serviceClient;
        }
    }
}