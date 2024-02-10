using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGamesRepository : IGamesRepository
{
    private readonly List<Game> games = new(){
    new Game(){
        Id = 1,
        Name = "Age of Empires",
        Genre = "Strategy",
        Price = 45.99M,
        ReleaseDate = new DateTime(2020,08,18),
        ImageUri = "https://placehold.co/100"
    },
    new Game(){
        Id = 2,
        Name = "Final Fantasy",
        Genre = "Action",
        Price = 42.99M,
        ReleaseDate = new DateTime(2015,08,18),
        ImageUri = "https://placehold.co/100"
    },
    new Game(){
        Id = 3,
        Name = "Crisis",
        Genre = "Fighting",
        Price = 42.99M,
        ReleaseDate = new DateTime(2009,08,18),
        ImageUri = "https://placehold.co/100"
    },
    new Game(){
        Id = 4,
        Name = "Loard of the Rings",
        Genre = "Role Play",
        Price = 42.99M,
        ReleaseDate = new DateTime(2021,08,18),
        ImageUri = "https://placehold.co/100"
    }
};
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await Task.FromResult(games);
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await Task.FromResult(games.Find(x => x.Id == id));
    }

    public async Task CreateAsync(Game game)
    {
        game.Id = games.Max(x => x.Id) + 1;
        games.Add(game);

        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Game updatGame)
    {
        int index = games.FindIndex(x => x.Id == updatGame.Id);
        games[index] = updatGame;

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        int index = games.FindIndex(x => x.Id == id);
        games.RemoveAt(index);

        await Task.CompletedTask;
    }
}