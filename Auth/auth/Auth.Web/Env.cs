using System;
using System.IO;

namespace AuthorizationServer.Web;

public static class Env
{
    public static class Postgres
    {
        public const string Host = "POSTGRES_HOST";
        public const string Port = "POSTGRES_PORT";
        public const string Db = "POSTGRES_DB";
        public const string User = "POSTGRES_USER";
        public const string Password = "POSTGRES_PASSWORD";
    }

    public static string? Get(string key) => Environment.GetEnvironmentVariable(key);

    public static void LoadFromCurrentDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var env = $"{currentDirectory}/postgres.env";
        Load(env);
    }
    
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}