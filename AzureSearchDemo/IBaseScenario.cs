namespace AzureSearchDemo
{
    public interface IBaseScenario
    {
        void Init(string searchServiceName, string adminApiKey, string indexName);
        void Start();
    }
}