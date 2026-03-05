var client = new HttpClient();

var response = await client.GetAsync("https://localhost:17225/");

response = await client.PostAsync("https://localhost:17225/game/new", new StringContent("word"));
//var body = await response.Content.ReadAsStringAsync();

//Console.WriteLine($"{(int)response.StatusCode}: {response.StatusCode}");
//Console.WriteLine(body);

//Console.ReadLine();

