using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
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
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapGet("/", (GameStoreContext dbContext) =>
            {
                Results.Ok(dbContext.Games);
            });

            // Get games by id /get/{id}
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapGet("/{id:int}", ([FromRoute] int id, GameStoreContext dbContext) =>
            {
                /*
                GameDto? gameDto = games.Find(x => x.Id == id);
                return gameDto == null ? Results.NotFound(gameDto) : Results.Ok(gameDto);
                */

                Game gameEntity = dbContext.Games.Find(id);
                return gameEntity == null ? Results.NotFound(gameEntity) : Results.Ok(gameEntity);
            })
            .WithName(GetGameEntryPointName);

            // Post games (body = create game dto
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapPost("/", ([FromBody] CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                /*
                int newId = games.Max(i => i.Id) + 1;
                GameDto gameDto = new GameDto(newId, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
                games.Add(gameDto);
                */

                // Replace by extended CreateContextDto class
                // Create Game entity
                /*
                int newId = dbContext.Games.Max(i => i.Id) + 1;
                Game game = new()
                {
                    // Without table link
                    Id = newId,
                    Name = newGame.Name,
                    Price= newGame.Price,
                    ReleaseDate = newGame.ReleaseDate,  

                    // For genre and table we need now to change Created DTO because genre is a link
                    // Save id
                    GenreId = newGame.GenreId,
                    //Genre we need to get info from table genre
                    Genre = dbContext.Genres.Find(newGame.GenreId)
                };
                */
                Game game = newGame.ToEntity();
                if (dbContext.Games.Count() == 0)
                    game.Id = 1;
                else
                    game.Id = dbContext.Games.Max(i => i.Id) + 1;
                game.Genre = dbContext.Genres.Find(newGame.GenreId);

                // Add object i db
                dbContext.Games.Add(game);

                // Save DB
                dbContext.SaveChanges();

                //Replace code by game mapping fonction
                /*
                // To avoid to change contract with client we need to atranform enity in DTO
                GameDto gameDto = new
                (
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                );
                */
                //We cn directly returned in return
                GameDto gamedto = game.ToDto();


                // Return DTO
                return Results.CreatedAtRoute(
                    GetGameEntryPointName, 
                    new { id = game.Id },
                    game.ToDto()
                );
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
