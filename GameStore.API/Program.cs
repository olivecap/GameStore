using GameStore.API.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//------------------------------
// Create record in memeory
//------------------------------

List<GameDto> games = [
        new (
            1,
            "Street Figther II",
            "Figthing",
            19.99M,
            new DateOnly(1992, 7, 14)),
        new (
            2,
            "Final Fabtasy XIV",
            "Figthing",
            59.99M,
            new DateOnly(2000, 10, 30)),
        new (
            3,
            "FIFA 2023",
            "Figthing",
            69.99M,
            new DateOnly(2022, 9, 27))
    ];

app.MapGet("/", () => games);

app.Run();
