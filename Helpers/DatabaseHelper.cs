using CsvHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SQLite;
using System.IO;

namespace AtmWebAppTesting.Database
{
    public class DatabaseHelper
    {
        private readonly string connectionString;
        
        public DatabaseHelper(string databaseFilePath)
        {
            connectionString = $"Data Source={databaseFilePath};Version=3;";
        }

        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch
            {
                Console.WriteLine("Database unable to open");
            }
            return connection;
        }

        public void CreateNewDatabase(string databaseName)
        {
            _ = new SQLiteConnection($"Data Source = {databaseName}.db; Version = 3; New = True; Compress = True");
        }

        public void CreateNewTable(SQLiteConnection connection, string sqlQuery)
        {
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.ExecuteNonQuery();
        }

        public void InsertDataIntoTable(SQLiteConnection connection, string sqlQuery)
        {
            SQLiteCommand command;

            command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.ExecuteNonQuery();
        }

        public SQLiteDataReader ReadDataFromTable(SQLiteConnection connection, string tableName)
        {
            SQLiteDataReader reader;
            SQLiteCommand command;
            string sqlQuery = $"SELECT * FROM {tableName}";
            command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();
            
            return reader;
        }

        public void DeleteTable(SQLiteConnection connection, string tableName)
        {
            SQLiteCommand command;
            string sqlQuery = $"DROP TABLE IF EXISTS {tableName};";

            command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.ExecuteNonQuery();
            Console.WriteLine($"Table {tableName} has been deleted.");
        }

        public void CloseConnection(SQLiteConnection connection)
        {
            connection.Close();
        }

        public static string GetDatabasePath(string databaseName)
        {
            string databaseFilePath = Path.Combine(Environment.CurrentDirectory, $"{databaseName}.db");
            return databaseFilePath;
        }

    }
}
