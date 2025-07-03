namespace GameStore.API.Entities
{
    /// <summary>
    /// Entity for table genre
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
