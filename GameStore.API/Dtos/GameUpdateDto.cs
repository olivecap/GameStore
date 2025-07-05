using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos
{
    //---------------------------------------------
    // Even if same Dto of create
    // By convention is better to have DTO
    // for each action in case of it will change
    //----------------------------------------------
    /// <summary>
    /// DTO for game
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="GenreId"></param>
    /// <param name="Price"></param>
    /// <param name="ReleaseDate"></param>
    public record class GameUpdateDto(
        [Required]
        [StringLength(100, MinimumLength = 1)]
        string Name,
        [Required]
        int GenreId,
        [Required]
        [Range(0, 200)]
        decimal Price,
        DateOnly ReleaseDate);
}
