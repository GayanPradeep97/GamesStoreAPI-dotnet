using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api;

public static class GameEndpoints
{

    const string GetEndpointName = "GetGames";

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games").WithParameterValidation();

        //Get / games
        group.MapGet("/", async (GameStoreContext dbContext) =>
       await dbContext.Games.Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto()).AsNoTracking().ToListAsync()
        );  //finally i added this ToListAsync() method to code async. async and await used.. all of the codes used that


        //Get / games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {

            Game? game = await dbContext.Games.FindAsync(id);


            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetEndpointName);



        //post/games
        group.MapPost("/", async (CreateGamesDto newGame, GameStoreContext dbContext) =>
        {

            Game game = newGame.ToEntity();
            game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);


            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();


            return Results.CreatedAtRoute(GetEndpointName, new { id = game.Id }, game.ToGameDetailsDto());

        });



        //update/games/{id}
        group.MapPut("/{id}", async (int id, UpdateDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });



        //Delete/games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();


            return Results.NoContent();
        });

        return group;
    }

}
