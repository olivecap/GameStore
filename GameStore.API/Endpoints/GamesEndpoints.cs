using GameStore.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace GameStore.API.Endpoints
{
    /// <summary>
    /// Prepare extension to create specific code in specific file
    /// </summary>
    public static class GamesEndpoints
    {
        //------------------------------
        // Csts
        //------------------------------
        const string GetGameEntryPointName = "GetName";

        //------------------------------
        // Create record in memeory
        //------------------------------
        private readonly static List<GameDto> games = [
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

        //---------------------
        //  Extension class
        //---------------------
        /// <summary>
        /// Create an extension for the WebApplication
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            //---------------------------------------------------------
            // Add group root to avaoid to add each time route (games)
            // Use group in each endpoint
            // Avoid to recall /games  in each endpoints
            // Add validation of inut
            //---------------------------------------------------------
            var routeGroup = app.MapGroup("games")
                                .WithParameterValidation();

            //--------------------------------
            //- Add here all new endpoints
            //--------------------------------
            // Get all games /games
            routeGroup.MapGet("/", () => Results.Ok(games));

            // Get games by id /get/{id}
            routeGroup.MapGet("/{id:int}", ([FromRoute] int id) =>
            {
                GameDto? gameDto = games.Find(x => x.Id == id);
                return gameDto == null ? Results.NotFound(gameDto) : Results.Ok(gameDto);
            })
            .WithName(GetGameEntryPointName);

            // Post games (body = create game dto
            routeGroup.MapPost("/", ([FromBody] CreateGameDto newGame) =>
            {
                int newId = games.Max(i => i.Id) + 1;
                GameDto gameDto = new GameDto(newId, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
                games.Add(gameDto);
                return Results.CreatedAtRoute(GetGameEntryPointName, new { id = newId }, gameDto);
            });

            // Post games (body = create game dto
            routeGroup.MapPut("/{id:int}", ([FromRoute] int id, [FromBody] UpdateGameDto updateGame) =>
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
            routeGroup.MapDelete("/{id:int}", ([FromRoute] int id) =>
            {
                var gameDto = games.Find(x => x.Id == id);
                if (gameDto == null)
                    return Results.NotFound(gameDto);

                games.Remove(gameDto);
                return Results.NoContent();
            });

            // Return extension class
            return routeGroup;
        }
    }
}
