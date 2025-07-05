using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        // Replace by db object
        /*
        private readonly static List<GameSummaryDto> games = [
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
        */

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
            //----------------------------------
            //      GET ENDPOINT
            //----------------------------------
            // Get all games /games
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapGet("/", (GameStoreContext dbContext) =>
            {
                //Results.Ok(games);
                return Results.Ok(dbContext.Games
                    // Allow to specify we also use gGenre table
                    .Include(game => game.Genre)
                    // Select each game
                    .Select(game => game.ToGameSummaryDto())
                    // Optimization to avoid to track modification because one shot action
                    .AsNoTracking());
            });

            //----------------------------------
            //      GET BY ID ENDPOINT
            //----------------------------------
            // Get games by id /get/{id}
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapGet("/{id:int}", ([FromRoute] int id, GameStoreContext dbContext) =>
            {
                /*
                GameDto? gameDto = games.Find(x => x.Id == id);
                return gameDto == null ? Results.NotFound(gameDto) : Results.Ok(gameDto);
                */

                // Get entity object
                Game? gameEntity = dbContext.Games.Find(id);
                if (gameEntity == null)
                    return Results.NotFound();

                // Transform 
                return gameEntity == null ? Results.NotFound() : Results.Ok(gameEntity.ToGameDetailsDto());
            })
            .WithName(GetGameEntryPointName);

            //----------------------------------
            //      POST ENDPOINT
            //----------------------------------
            // Post games (body = create game dto
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapPost("/", ([FromBody] GameCreateDto newGame, GameStoreContext dbContext) =>
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

                // Generate game entity
                Game game = newGame.ToEntity(dbContext);

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
                
                // Return DTO
                return Results.CreatedAtRoute(
                    GetGameEntryPointName, 
                    new { id = game.Id },
                    game.ToGameDetailsDto()
                );
            });

            //----------------------------------
            //      PUT ENDPOINT
            //----------------------------------
            // Post games (body = create game dto
            routeGroup.MapPut("/{id:int}", ([FromRoute] int id, [FromBody] GameUpdateDto updateGame, GameStoreContext dbContext) =>
            {
                /*
                int index = games.FindIndex(x => x.Id == id);
                if (index == -1)
                    return Results.NotFound(id);

                // Record if not modifiable
                // Reason why we decide to create a new object at the same index
                games[index] = new GameSummaryDto(
                        id,
                        updateGame.Name,
                        updateGame.Genre,
                        updateGame.Price,
                        updateGame.ReleaseDate
                    );
                return Results.NoContent();
                */
                // Imporvment with dbContext
                Game? game = dbContext.Games.Find(id);
                if (game == null)
                    return Results.NotFound();

                // Update entity
                // We need to lock entity
                dbContext.Entry(game)
                    .CurrentValues
                    .SetValues(updateGame.ToEntity(game.Id));

                // Save DB
                dbContext.SaveChanges();

                return Results.Ok(game.ToGameUpdateDto());
            });

            //----------------------------------
            //      DELETE ENDPOINT
            //----------------------------------
            // Delete games by id /delete/{id}
            routeGroup.MapDelete("/{id:int}", ([FromRoute] int id, GameStoreContext dbContext) =>
            {
                /*
                var gameDto = games.Find(x => x.Id == id);
                if (gameDto == null)
                    return Results.NotFound(gameDto);

                games.Remove(gameDto);
                return Results.NoContent();
                */

                /*
                Game? game = dbContext.Games.Find(id);
                if (game is null)
                    return Results.NotFound();

                // Remove object
                dbContext.Remove(game);

                // Save db
                dbContext.SaveChanges();
                */

                Game? game = dbContext.Games.Find(id);
                if (game is null)
                    return Results.NotFound();

                // More optimized
                dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDelete();

                return Results.NoContent();
            });

            // Return extension class
            return routeGroup;
        }
    }
}
