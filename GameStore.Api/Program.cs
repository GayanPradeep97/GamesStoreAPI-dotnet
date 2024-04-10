using GameStore.Api.Dtos;


const string GetEndpointName = "GetGames";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new(
        1,
        "Street Fighter II",
        "Fighting",
        16.99M,
        new DateOnly(1992,7,15)
    ),
     new(
        2,
        "Final Fanstasy XIV",
        "Roleplaying",
        59.99M,
        new DateOnly(1910,9,03)
    ),
     new(
        3,
        "FIFA 23",
        "Sports",
        69.99M,
        new DateOnly(2022,9,27)
    ),
     new(
        4,
        "Sprty Game",
        "Sports",
        16.99M,
        new DateOnly(2023,2,15)
    )
];

//GET / games
app.MapGet("games", () => games);

//Get / games/1
app.MapGet("games/{id}", (int Id) => games.Find(game => game.Id == Id)).WithName(GetEndpointName);


//post/games
app.MapPost("games", (CreateGamesDto newGame) =>
{


    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetEndpointName, new { id = game.Id }, game);

});

//update/games/{id}
app.MapPut("games/{id}", (int id, UpdateDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );
    return Results.NoContent();
});

app.Run();
