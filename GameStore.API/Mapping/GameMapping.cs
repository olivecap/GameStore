using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Runtime.CompilerServices;

namespace GameStore.API.Mapping
{
    public static class GameMapping
    {
        /// <summary>
        /// Extend 
        /// </summary>
        /// <param name="createGameDto"></param>
        /// <returns></returns>
        public static Game ToEntity(this GameCreateDto createGameDto, GameStoreContext dbContext)
        {
            // Create Game entity
            // Missing Id and genre coming from dbContext
            return new Game()
            {
                // Without table link
                Name = createGameDto.Name,
                Price = createGameDto.Price,
                ReleaseDate = createGameDto.ReleaseDate,
                GenreId = createGameDto.GenreId,
                Id = dbContext.GenerateGameId(),
                Genre = dbContext.Genres.Find(createGameDto.GenreId)
            };

        }

        /// <summary>
        /// Extend 
        /// </summary>
        /// <param name="createGameDto"></param>
        /// <returns></returns>
        public static Game ToEntity(this GameUpdateDto updateGameDto, int id)
        {
            // Create Game entity
            return new Game()
            {
                // Without table link
                Name = updateGameDto.Name,
                Price = updateGameDto.Price,
                ReleaseDate = updateGameDto.ReleaseDate,
                GenreId = updateGameDto.GenreId,
                Id = id
            };

        }

        /// <summary>
        /// Extend game enntity to return DTO
        /// </summary>
        /// <param name="gameEntity"></param>
        /// <returns></returns>
        public static GameSummaryDto ToGameSummaryDto(this Game gameEntity)
        {
            // Get dbcontext from injection
            
            return new
            (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.Genre!.Name,
                gameEntity.Price,
                gameEntity.ReleaseDate
            );
        }

        public static GameDetailsDto ToGameDetailsDto(this Game gameEntity)
        {
            // Get dbcontext from injection

            return new
            (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.GenreId,
                gameEntity.Price,
                gameEntity.ReleaseDate
            );
        }

        public static GameUpdateDto ToGameUpdateDto(this Game gameEntity)
        {
            // Get dbcontext from injection

            return new
            (
                gameEntity.Name,
                gameEntity.GenreId,
                gameEntity.Price,
                gameEntity.ReleaseDate
            );
        }
    }
}
