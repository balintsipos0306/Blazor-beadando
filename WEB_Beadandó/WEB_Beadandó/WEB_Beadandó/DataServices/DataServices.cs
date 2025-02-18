using Dapper;
using Microsoft.Data.SqlClient;

namespace WEB_Beadandó.DataServices;

internal sealed class DataServices(ILogger<DataServices> Logger, IConfiguration configuration) : IDataServices
{
    public async Task<IEnumerable<T>> GetList<T>(string cmd, object? qParam = null)
    {
        try
        {
            using var con = new SqlConnection(configuration.GetConnectionString("DbConn"));
            await con.OpenAsync();
            return await con.QueryAsync<T>(cmd, qParam);

        }
        catch (Exception ex)
        {
            Logger.LogCritical("Error message: {ex}, {cmd}", ex, cmd);
            return [];

        }
    }

    public async Task<T?> GetById<T>(int id) where T : class
    {
        try
        {
            using var con = new SqlConnection(configuration.GetConnectionString("DbConn"));
            await con.OpenAsync();
            // Alapértelmezett lekérdezés, ahol az 'Id' a paraméter, amit az adatmodell osztálya alapján adaptálhatsz.
            var cmd = $"SELECT * FROM [Haszaltauto].[User].[{typeof(T).Name}] WHERE  {typeof(T).Name.ToLower()}Id = @Id";
            return await con.QueryFirstOrDefaultAsync<T>(cmd, new { Id = id });
        }
        catch (Exception ex)
        {
            Logger.LogCritical("Error message: {ex}, GetById command", ex);
            return null;  // Visszaadja a null értéket, ha nem található rekord
        }
    }

    public async Task<int> Execute(string cmd, object? qParam = null)
    {
        try
        {
            using var con = new SqlConnection(configuration.GetConnectionString("DbConn"));
            await con.OpenAsync();
            return await con.ExecuteAsync(cmd, qParam);
        }
        catch (Exception ex)
        {
            Logger.LogCritical("Error message: {ex}, Execute command", ex);
            return 0;  // Ha hiba történik, 0-t adunk vissza
        }
    }
}
