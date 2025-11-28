using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GGStat_Backend.Data;

public static class DbUtils
{
    public static async Task KillConnectionsAsync(DbContext db, string Database)
    {
        var conn = (NpgsqlConnection)db.Database.GetDbConnection();

        var builder = new NpgsqlConnectionStringBuilder(conn.ConnectionString) { Database = Database };
        await using var admin = new NpgsqlConnection(builder.ToString());
        await admin.OpenAsync();

        var dbName = conn.Database;
        var sql = """
                  SELECT pg_terminate_backend(pid)
                  FROM pg_stat_activity
                  WHERE datname = @db AND pid <> pg_backend_pid()
                  """;
        await using var cmd = new NpgsqlCommand(sql, admin);
        cmd.Parameters.AddWithValue("db", dbName);
        await cmd.ExecuteNonQueryAsync();
    }
}