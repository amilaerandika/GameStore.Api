using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var conString = configuration.GetConnectionString("GameStoreContext");
        services.AddSqlServer<GameStoreContext>(conString)
                .AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();
        return services;

    }
}