namespace GameStore.API.Entities
{
    /// <summary>
    /// Game entity
    /// </summary>
    public class Game
    {
        //---------------------------------
        //  Games parameters
        //---------------------------------
        public int Id { get; set; }
        public required string Name { get; set; }   
        public required decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }

        //------------------------------------
        // Link to genre table
        //------------------------------------
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
