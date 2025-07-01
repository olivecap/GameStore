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
        string Name,
        string Genre,
        decimal Price,
        DateOnly ReleaseDate);
}
