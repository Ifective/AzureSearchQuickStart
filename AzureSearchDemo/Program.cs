using System;
using System.Linq;
using Fclp;

namespace AzureSearchDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new FluentCommandLineParser<ApplicationArguments>();
                arguments.Setup(arg => arg.SearchServiceName)
                    .As('s', "searchservice")
                    .Required();
                arguments.Setup(arg => arg.AdminApiKey)
                    .As('k', "key")
                    .Required();
                arguments.Setup(arg => arg.IndexName)
                    .As('i', "indexname")
                    .Required();            
                arguments.Parse(args);

            ScenarioFactory.Current.Initialize();
            ShowOptions(arguments.Object.SearchServiceName, arguments.Object.AdminApiKey, arguments.Object.IndexName);
        }


        private static void ShowOptions(string searchServiceName, string adminApiKey, string indexName)
        {
            var letter = 'a';
            var options = ScenarioFactory.Current.GetScenarios().ToDictionary(serviceBase => letter++);

            char choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose a scenario:");
                foreach (var option in options)
                {
                    Console.WriteLine($"{option.Key}:\t{option.Value}");
                }
                Console.WriteLine("z:\tExit");

                choice = Console.ReadKey().KeyChar;
                if (!options.ContainsKey(choice))
                {
                    continue;
                }
                var scenarioName = options[choice];
                if (string.IsNullOrEmpty(scenarioName))
                {
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"Scenario {scenarioName} is being started");
                try
                {
                    ScenarioFactory.Current.CreateScenario(scenarioName, searchServiceName, adminApiKey, indexName).Start();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error: {exception}");
                }
                Console.WriteLine($"Scenario {scenarioName} finished");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } while (choice != 'z');
        }


    }

    public class ApplicationArguments
    {
        public string Scenario { get; set; }
        public string SearchServiceName { get; set; }
        public string AdminApiKey { get; set; }
        public string IndexName { get; set; }
    }
}
