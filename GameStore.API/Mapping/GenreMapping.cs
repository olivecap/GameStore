using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Runtime.CompilerServices;

namespace GameStore.API.Mapping
{
    public static class GenreMapping
    {
        public static GenreDto ToGenreDto(this Genre genreEntity)
        {
            // Get dbcontext from injection

            return new
            (
                genreEntity.Id,
                genreEntity.Name
            );
        }
    }
}
