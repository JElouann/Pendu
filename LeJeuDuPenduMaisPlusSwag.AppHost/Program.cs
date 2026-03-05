using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#region Swagger
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}
#endregion

#region Database


using var connection = new SqliteConnection("Data Source=..\\database.db");

connection.Open();

var command = connection.CreateCommand();
SqliteDataReader reader;

// Handles the case where the table already exists
try
{
    command.CommandText = """
    CREATE TABLE "games" (
    	"id"	INTEGER NOT NULL,
    	"word"	text NOT NULL,
    	"state"	int,
    	PRIMARY KEY("id" AUTOINCREMENT)
    )
    CREATE TABLE "attempts" (
    	"word"	text,
    	"gameId"	INTEGER,
    	FOREIGN KEY("gameId") REFERENCES "games"("id")
    )
    """;

    reader = command.ExecuteReader();
}
catch
{
    // Resets tables, as well as auto_incremented id
    command.CommandText = """
        delete from attempts;
        delete from games;

        delete from sqlite_sequence where name='games';
        delete from sqlite_sequence where name='attempts';
        """;

    reader = command.ExecuteReader();
}

#endregion
#region PreProd

#region Consignes
// API Rest en C#
// Jeu du pendu
// un swagger
// Routes : 
// - une route pour créer une partie avec en paramètre le mot à faire deviner
// - une route pour lister les parties en cours
// - une route pour proposer une lettre dans une partie 
// - une route pour proposer un mot dans une partie
// - une route pour supprimer une partie
// - une route pour lister les parties terminées
// - une route pour accéder à l'historique d'une partie terminée
// Base de données
// - stocker la liste des parties avec leur ID, le mot à deviner ainsi que leur état (en cours, victoire, défaite)
// - stocker la liste de chacun des essais
#endregion

#region Notes
// - Post
// - Get
// - Post
// - Post
// - Post
// - Get
// - Get
#endregion

#endregion


#region Nouveau

string authorizedGuess = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
string wordToFind = "";
List<string> pastTries = new();
Dictionary<int, Game> games = new();

// Create game with SQL
app.MapPost("/game/new", (string word) =>
{
    // Verify if word is valid
    foreach (char c in word.ToCharArray())
    {
        if (!authorizedGuess.Contains(c))
        {
            Console.WriteLine("Word is not valid.");
            return "Word is not valid.";
        }
    }

    //INSERT VALUES
    command = connection.CreateCommand();
    command.CommandText = """
        insert into games (word, state) values ($word, 0);
        """;

    command.Parameters.AddWithValue("$word", word);

    reader = command.ExecuteReader();
    wordToFind = word;
    return $"Welcome in game. The word to find is {word}";
});

// Get games with SQL
app.MapGet("/game/games", () =>
{
    // SELECT
    command = connection.CreateCommand();
    command.CommandText = """
        select id
        from games
    """;

    using var reader3 = command.ExecuteReader();

    string id = "";
    while (reader3.Read())
    {
        id += reader3.GetString(0) + "\n";

        Console.WriteLine($"{id}");
    }
    return $"{id}";
});


// Guess letter
app.MapPost("/game/guessLetter", (string letter, int id) =>
{
    // Verify if letter is valid
    if (letter.Length > 1 || !authorizedGuess.Contains(letter))
    {
        Console.WriteLine("Letter is not valid.");
        return "Letter is not valid.";
    }

    // Add letter to Attempts table
    command = connection.CreateCommand();
    command.CommandText = """
        insert into attempts (word, gameId) values ($letter, $id);
    """;

    command.Parameters.AddWithValue("$letter", letter);
    command.Parameters.AddWithValue("$id", id);
    
    reader = command.ExecuteReader();

    // Check if guess is good or bad
    command = connection.CreateCommand();
    command.CommandText = """
        select word from Games where id = $id;
    """;

    command.Parameters.AddWithValue("$id", id);

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        if (reader.GetString(0).Contains(letter)) return "True";
        else return "False";
    }

    return "";
});

// Guess word
app.MapPost("/game/guessWord", (string word, int id) =>
{
    // Verify if word is valid
    foreach (char c in word.ToCharArray())
    {
        if (!authorizedGuess.Contains(c))
        {
            Console.WriteLine("Word is not valid.");
            return "Word is not valid.";
        }
    }

    // Add word to Attempts table
    command = connection.CreateCommand();
    command.CommandText = """
        insert into attempts (word, gameId) values ($word, $id);
    """;

    command.Parameters.AddWithValue("$word", word);
    command.Parameters.AddWithValue("$id", id);

    reader = command.ExecuteReader();

    // Check if guess is good or bad
    command = connection.CreateCommand();
    command.CommandText = """
        select word from Games where id = $id;
    """;

    command.Parameters.AddWithValue("$id", id);

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        if (reader.GetString(0) == word) return $"Good job !";
        else return $"False";
    }
    return "";
});

// History
app.MapGet("/game/history", () =>
{
    string triesList = "Current game's history: \n";

    // SELECT
    command = connection.CreateCommand();
    command.CommandText = """
        select word
        from attempts
    """;

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        triesList += reader.GetString(0) + "\n";
    }
    return $"{triesList}";
});

app.Run();
record Game(string Word, int ID);
#endregion

