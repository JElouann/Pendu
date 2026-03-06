var client = new HttpClient();

var response = await client.GetAsync("https://localhost:17225/");
var body = await response.Content.ReadAsStringAsync();


//var body = await response.Content.ReadAsStringAsync();

//Console.WriteLine($"{(int)response.StatusCode}: {response.StatusCode}");
//Console.WriteLine(body);

//Console.ReadLine();

Console.WriteLine("Welcome to Zi Pendu, choose an option :\n" +
                  "-\"new\" to create a new game\n" +
                  "-\"games\" to see already existing games\n" +
                  "-\"guessletter\" to make a guess in a game\n" +
                  "-\"guessword\" to guess the word of a game\n" +
                  "-\"delete\" to delete a game\n" +
                  "-\"history\" to see the history of a game\n");

while (true)
{

    string command = Console.ReadLine();
    string lastCommand = "";

    int gameID = 0;
    string guess = "";

    Console.WriteLine("\n");

    switch (command)
    {
        case "new":
            Console.WriteLine("Choose your word to guess :");

            string parameter = Console.ReadLine();
            Console.WriteLine("\n");

            response = await client.PostAsync($"https://localhost:17225/game/new?word={parameter}", null);
            body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            lastCommand = "new";
            break;

        case "games":
            response = await client.GetAsync($"https://localhost:17225/game/games");
            body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            Console.WriteLine("\n");
            break;

        case "guessletter":
            Console.WriteLine("Choose the game by ID :");

            if (Int32.TryParse(Console.ReadLine(), out gameID))
            {
                Console.WriteLine("\n");

                Console.WriteLine("Choose your letter to guess :");
                guess = Console.ReadLine();
                Console.WriteLine("\n");
            }

            response = await client.PostAsync($"https://localhost:17225/game/guessLetter?letter={guess}&id={gameID}", null);
            body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            break;

        case "guessword":
            Console.WriteLine("Choose the game by ID :");

            if (Int32.TryParse(Console.ReadLine(), out gameID))
            {
                Console.WriteLine("\n");

                Console.WriteLine("Choose your word to guess :");
                guess = Console.ReadLine();
                Console.WriteLine("\n");
            }

            response = await client.PostAsync($"https://localhost:17225/game/guessWord?word={guess}&id={gameID}", null);
            body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            break;

        case "delete":
            Console.WriteLine("Choose the game by ID :");

            if (Int32.TryParse(Console.ReadLine(), out gameID))
            {
                response = await client.PostAsync($"https://localhost:17225/game/delete?id={gameID}", null);
                body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
                break;

        case "history":
            Console.WriteLine("Choose the game by ID :");

            if (Int32.TryParse(Console.ReadLine(), out gameID))
            {
                response = await client.GetAsync($"https://localhost:17225/game/history?id={gameID}");
                body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            break;

        default:
            Console.WriteLine("Unknown command.\n");
            Console.WriteLine("Choose an option :\n" +
                  "-\"new\" to create a new game\n" +
                  "-\"games\" to see already existing games\n" +
                  "-\"guessletter\" to make a guess in a game\n" +
                  "-\"guessword\" to guess the word of a game\n" +
                  "-\"delete\" to delete a game\n" +
                  "-\"history\" to see the history of a game\n");
            break;
    }

    //Console.WriteLine("Press enter to repeat.\n");

}

