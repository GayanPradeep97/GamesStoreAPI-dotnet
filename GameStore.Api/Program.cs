using GameStore.Api;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

await app.MigrateDbAsync();
app.MapGenresEndpoints();
app.MapGameEndpoints();

app.Run();
