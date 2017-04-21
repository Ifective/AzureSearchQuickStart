using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AzureSearchDemo.Scenarios;

namespace AzureSearchDemo
{
    public class ScenarioFactory
    {
        private Dictionary<string, Type> _scenarios;

        private static volatile ScenarioFactory _instance;
        private static readonly object SyncRoot = new Object();
        public static ScenarioFactory Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ScenarioFactory();
                    }
                }

                return _instance;
            }
        }

        private ScenarioFactory()
        {
        }

        public IEnumerable<string> GetScenarios() => _scenarios?.Keys;

        public void Initialize()
        {
            _scenarios = Assembly.GetAssembly(typeof(BaseSearchScenario)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && (myType.IsSubclassOf(typeof(BaseSearchScenario)) || myType.IsSubclassOf(typeof(BaseSuggestScenario))))
                .ToDictionary(t => t.Name);
        }

        public IBaseScenario CreateScenario(string scenarioKey, string searchServiceName, string adminApiKey, string indexName)
        {
            Type resultType;
            if (!_scenarios.TryGetValue(scenarioKey, out resultType))
                throw new InvalidOperationException("Scenario not found");

            var result = (IBaseScenario)Activator.CreateInstance(resultType);
            result.Init(searchServiceName, adminApiKey, indexName);
            return result;
        }

    }
}