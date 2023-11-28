// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using RestServerSimulatorTest;


string[] FirstNames = { "Leia", "Sadie", "Jose", "Sara", 
    "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
string[] LastNames = { "Liberty", "Ray", "Harrison", "Ronan", "Drew",
    "Powell", "Larsen", "Chan", "Anderson", "Lane" };

var httpClient = new HttpClient();
var random = new Random();

// Simulate parallel POST requests
var postTasks = new List<Task>();
for (var i = 0; i <= 2; i++)
{
    var newCustomers = new List<CustomerRequestModel>
    {
        CreateRandomCustomer(random),
        CreateRandomCustomer(random)
    };

    postTasks.Add(SendPostRequestAsync(httpClient, newCustomers));
}

await Task.WhenAll(postTasks);

// Simulate GET request
var getResponse = await httpClient.GetAsync("http://localhost:5001/customer/customers");
getResponse.EnsureSuccessStatusCode();

var customers = await getResponse.Content.ReadFromJsonAsync<List<CustomerRequestModel>>();
foreach (var customer in customers)
{
    Console.WriteLine($"{customer.LastName} {customer.FirstName}, Age: {customer.Age}, ID: {customer.Id}");
}


static async Task SendPostRequestAsync(HttpClient httpClient, List<CustomerRequestModel> newCustomers)
{
    try
    {
        var response = await httpClient.PostAsJsonAsync("http://localhost:5001/customer/customers", newCustomers);
        response.EnsureSuccessStatusCode();
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Request failed: {ex.Message}");
    }
}


CustomerRequestModel CreateRandomCustomer(Random randomVar)
{
    var usedIds = new HashSet<int>(); // Keep track of used IDs

    var firstName = FirstNames[randomVar.Next(FirstNames.Length)];
    var lastName = LastNames[randomVar.Next(LastNames.Length)];
    var age = randomVar.Next(10, 91); // Random age between 10 and 90
    int id;
    do
    {
        id = randomVar.Next(1, int.MaxValue);
    } while (!usedIds.Add(id)); 

    return new CustomerRequestModel
    {
        Id = id,
        FirstName = firstName,
        LastName = lastName,
        Age = age
    };
}
