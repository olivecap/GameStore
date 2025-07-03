using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos
{
    /// <summary>
    /// DTO for game
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Genre"></param>
    /// <param name="Price"></param>
    /// <param name="ReleaseDate"></param>
    public record class CreateGameDto(
        [Required]
        [StringLength(100, MinimumLength = 1)]
        string Name,
        [Required]
        [StringLength(50)]
        string Genre,
        [Required]
        [Range(0, 200)]
        decimal Price,
        DateOnly ReleaseDate);
}
