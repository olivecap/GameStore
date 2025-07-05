using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Endpoints;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//-----------------------------------------
// SDLite connector using injection
// You can spcify directory if you want
// Name of the db
//-----------------------------------------
// Connection string 
var connectionSqlDb = builder.Configuration.GetConnectionString("GameStore");
// DB request
builder.Services.AddSqlite<GameStoreContext>(connectionSqlDb);

var app = builder.Build();

//--------------------------------------------
//  USE EXTENSION APP TO ADD GAMES ENDPOINTS
//--------------------------------------------
app.MapGamesEndpoints();

// Run migartion
await app.MigrateDBAsync();

// Run app
app.Run();