namespace GameStore.API.Dtos
{
    /// <summary>
    /// DTO for game details containing genreId
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="GenreID"></param>
    /// <param name="Price"></param>
    /// <param name="ReleaseDate"></param>
    public record class GameDetailsDto(
        int Id, 
        string Name, 
        int GenreID, 
        decimal Price, 
        DateOnly ReleaseDate);
}
