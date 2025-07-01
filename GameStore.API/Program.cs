using GameStore.API.Dtos;
using GameStore.API.Endpoints;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//--------------------------------------------
//  USE EXTENSION APP TO ADD GAMES ENDPOINTS
//--------------------------------------------
app.MapGamesEndpoints();  

// Run app
app.Run();