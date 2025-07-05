using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;
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
        public static Game ToEntity(this CreateGameDto createGameDto)
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
            };

        }
        /// <summary>
        /// Extend game enntity to return DTO
        /// </summary>
        /// <param name="gameEntity"></param>
        /// <returns></returns>
        public static GameDto ToDto(this Game gameEntity)
        {
            return new
            (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.Genre!.Name,
                gameEntity.Price,
                gameEntity.ReleaseDate
            );
        }
    }
}
