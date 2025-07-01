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
    /// <param name="Genre"></param>
    /// <param name="Price"></param>
    /// <param name="ReleaseDate"></param>
    public record class UpdateGameDto(
        string Name,
        string Genre,
        decimal Price,
        DateOnly ReleaseDate);
}
