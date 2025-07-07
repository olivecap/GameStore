using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace GameStore.API.Endpoints
{
    /// <summary>
    /// Prepare extension to create specific code in specific file
    /// </summary>
    public static class GenreEndpoints
    {
        //------------------------------
        // Csts
        //------------------------------
        const string GetGameEntryPointName = "GetName";        

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
            var routeGroup = app.MapGroup("genres")
                                .WithParameterValidation();

            //--------------------------------
            //- Add here all new endpoints
            //--------------------------------
            //----------------------------------
            //      GET ENDPOINT
            //----------------------------------
            // Get all genres
            // Add db link using injection GameStoreContext dbContext
            routeGroup.MapGet("/", async (GameStoreContext dbContext) =>
            {
                var genreList = await dbContext.Genres
                    // Select each game
                    .Select(genre => genre.ToGenreDto())
                    // Optimization to avoid to track modification because one shot action
                    .AsNoTracking()
                    // ToListAsync allow to change in async mode
                    .ToListAsync();

                //Results.Ok(games);
                return  Results.Ok(genreList);
            });

            // Return extension class
            return routeGroup;
        }
    }
}
