using GameStore.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//------------------------------
// Csts
//------------------------------
const string GetGameEntryPointName = "GetName";

//------------------------------
// Create record in memeory
//------------------------------

List<GameDto> games = [
        new (
            1,
            "Street Figther II",
            "Figthing",
            19.99M,
            new DateOnly(1992, 7, 14)),
        new (
            2,
            "Final Fabtasy XIV",
            "Figthing",
            59.99M,
            new DateOnly(2000, 10, 30)),
        new (
            3,
            "FIFA 2023",
            "Figthing",
            69.99M,
            new DateOnly(2022, 9, 27))
    ];

// Get all games /games
app.MapGet("/games", () => Results.Ok(games));

// Get games by id /get/{id}
app.MapGet("/games/{id:int}", ([FromRoute] int id) =>
{
    GameDto? gameDto = games.Find(x => x.Id == id);
    return gameDto == null ? Results.NotFound(gameDto) : Results.Ok(gameDto);
})
.WithName(GetGameEntryPointName);

// Post games (body = create game dto
app.MapPost("games", ([FromBody] CreateGameDto newGame) =>
{
    int newId = games.Max(i => i.Id) + 1;
    GameDto gameDto = new GameDto  (newId, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate); 
    games.Add(gameDto);
    return Results.CreatedAtRoute(GetGameEntryPointName, new { id = newId }, gameDto);
});

// Post games (body = create game dto
app.MapPut("games/{id:int}", ([FromRoute] int id, [FromBody] UpdateGameDto updateGame) =>
{
    int index = games.FindIndex(x => x.Id == id);
    if (index == -1)
        return Results.NotFound(id);

    // Record if not modifiable
    // Reason why we decide to create a new object at the same index
    games[index] = new GameDto(
            id,
            updateGame.Name,
            updateGame.Genre,
            updateGame.Price,
            updateGame.ReleaseDate
        );
    return Results.NoContent();
});

// Delete games by id /delete/{id}
app.MapDelete("/games/{id:int}", ([FromRoute] int id) =>
{
    var gameDto = games.Find(x => x.Id == id);
    if (gameDto == null)
        return Results.NotFound(gameDto);

    games.Remove(gameDto);
    return Results.NoContent();
});

app.Run();