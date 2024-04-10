using GameStore.Api.Dtos;

namespace GameStore.Api;

public static class GameEndpoints
{

    const string GetEndpointName = "GetGames";
    private static List<GameDto> games = [
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

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games").WithParameterValidation();

        //Get / games
        group.MapGet("/", () => games);

        //Get / games/1
        group.MapGet("/{id}", (int Id) =>
        {
            GameDto? game = games.Find(game => game.Id == Id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetEndpointName);


        //post/games
        group.MapPost("/", (CreateGamesDto newGame) =>
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
        group.MapPut("/{id}", (int id, UpdateDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //Delete/games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);


            return Results.NoContent();
        });

        return group;
    }

}
