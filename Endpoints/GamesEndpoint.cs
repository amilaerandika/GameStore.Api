using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGameName";
    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/games").WithParameterValidation();
        // Configure the HTTP request pipeline.

        group.MapGet("/", async (IGamesRepository repository) =>
        (await repository.GetAllAsync()).Select(game => game.AsDto()));

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
        }
        ).WithName(GetGameEndpointName);

        group.MapPost("/", (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ImageUri = gameDto.ImageUri,
                ReleaseDate = gameDto.ReleaseDate
            };

            repository.CreateAsync(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", async (IGamesRepository repository, int id, UpdateGameDto updatedGame) =>
        {
            Game? existingGameObj = await repository.GetAsync(id);
            if (existingGameObj == null || updatedGame == null) return Results.NotFound();

            existingGameObj.Name = updatedGame.Name;
            existingGameObj.Genre = updatedGame.Genre;
            existingGameObj.Price = updatedGame.Price;
            existingGameObj.ReleaseDate = updatedGame.ReleaseDate;
            existingGameObj.ImageUri = updatedGame.ImageUri;

            await repository.UpdateAsync(existingGameObj);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? existingGame = await repository.GetAsync(id);

            if (existingGame is not null)
            {
                await repository.DeleteAsync(id);
                return Results.NoContent();
            }
            return Results.NotFound();

        });

        return group;
    }
}