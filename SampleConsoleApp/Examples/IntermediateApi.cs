using System.Net.Http.Json;

namespace SampleConsoleApp.Examples
{
    /// <summary>
    /// Using TheMealDB https://www.themealdb.com/api.php
    /// </summary>
    internal class IntermediateApi
    {
        public static void Start()
        {
            Console.WriteLine("Im at your service, what can I do for you?");
            Console.WriteLine("1: Suggest what to order");
            Console.WriteLine("2: List meals based on ingredient");
            Console.WriteLine("3: Search meal by name");

            var selection = Console.ReadLine();

            while (!int.TryParse(selection, out var result))
            {
                Console.WriteLine($"{result} is not a valid selection, only numeric values is accepted");
                Console.WriteLine($"Try again");

                selection = Console.ReadLine();
            }

            var apiConnector = new MyApiConnector();

            switch (int.Parse(selection))
            {
                case 1:
                    Console.WriteLine(apiConnector.GetRandomMeal());
                    break;
                case 3:
                    Console.WriteLine($"Enter name of meal");
                    var input = Console.ReadLine();
                    Console.WriteLine(apiConnector.GetMealByName(input));
                    break;
                default:
                    Console.WriteLine("Not implemented yet");
                    break;
            }

            Console.WriteLine("Press any key to start over. Press X to exit");

            var restart = Console.ReadLine();
            if (restart.ToLower() != "x")
                Start();
        }
    }

    internal class MyApiConnector
    {
        private readonly HttpClient _httpClient;

        public MyApiConnector()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.themealdb.com");
        }

        public string GetRandomMeal()
        {
            var result = _httpClient.GetFromJsonAsync<Meals>("api/json/v1/1/random.php", CancellationToken.None).GetAwaiter().GetResult();
            return result.meals.FirstOrDefault().strMeal;
        }

        public string GetMealByName(string input)
        {
            var result = _httpClient.GetFromJsonAsync<Meals>("api/json/v1/1/"+input+".php", CancellationToken.None).GetAwaiter().GetResult();

        }

    }

    public class Meals
    {
        public Meal[] meals { get; set; }
    }

    public class Meal
    {
        public string strMeal { get; set; }
    }
}
