using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Models;

namespace SavedResultManager
{
    public class ResultManager : IResultManager, IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;

        public ResultManager()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                //Use DB in project directory.  If it does not exist, create it:
                DataSource = "./SqliteDB.db"
            };

            _sqliteConnection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _sqliteConnection.Open();

            //Create a table (if not exists):
            var createTableCmd = _sqliteConnection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ProcessedPath (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Input TEXT NOT NULL, 
                    IsValid BOOLEAN NOT NULL CHECK (IsValid IN (0,1)), 
                    Result TEXT,
                    UNIQUE(Input)
                );";
            createTableCmd.ExecuteNonQuery();
        }

        public void AddResult(PathFinderResult model)
        {
            try
            {
                using var transaction = _sqliteConnection.BeginTransaction();
                var insertCmd = _sqliteConnection.CreateCommand();

                SetModelInsertCommand(insertCmd, model);

                transaction.Commit();
            }
            catch
            {
                //Ignore for now. TODO logging.
            }
        }

        public void BatchAddResult(List<PathFinderResult> modelList)
        {
            try
            {
                using var transaction = _sqliteConnection.BeginTransaction();

                foreach (var model in modelList)
                {
                    var insertCmd = _sqliteConnection.CreateCommand();
                    SetModelInsertCommand(insertCmd, model);
                }

                transaction.Commit();
            }
            catch
            {
                //Ignore for now. TODO logging.
            }
        }

        public List<PathFinderResult> GetAllResults()
        {
            var result = new List<PathFinderResult>();
            try
            {
                var command = _sqliteConnection.CreateCommand();
                command.CommandText = @"
                SELECT *
                FROM ProcessedPath;";

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapDbToModel(reader));
                }
            }
            catch
            {
                //Ignore for now. TODO logging.
            }

            return result;
        }

        public PathFinderResult GetResultById(int id)
        {
            PathFinderResult result = null;
            try
            {
                var command = _sqliteConnection.CreateCommand();
                command.CommandText = @"
                SELECT *
                FROM ProcessedPath
                WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = MapDbToModel(reader);
                }
            }
            catch
            {
                //Ignore for now. TODO logging.
            }

            return result;
        }

        public PathFinderResult GetResultByInput(string input)
        {
            PathFinderResult result = null;
            try
            {
                var command = _sqliteConnection.CreateCommand();
                command.CommandText = @"
                SELECT *
                FROM ProcessedPath
                WHERE Input = @input";
                command.Parameters.AddWithValue("@input", input);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = MapDbToModel(reader);
                }
            }
            catch
            {
                //Ignore for now. TODO logging.
            }

            return result;
        }

        private void SetModelInsertCommand(SqliteCommand command, PathFinderResult model)
        {
            command.CommandText = @"
                INSERT OR IGNORE INTO ProcessedPath(Input,IsValid,Result) 
                VALUES(@input, @isValid, @result)";
            command.Parameters.AddWithValue("@input", model.Input);
            command.Parameters.AddWithValue("@isValid", model.HasValidPath);
            if (!string.IsNullOrEmpty(model.MostEfficientPath))
            {
                command.Parameters.AddWithValue("@result", model.MostEfficientPath);
            }
            else
            {
                command.Parameters.AddWithValue("@result", DBNull.Value);
            }
            command.ExecuteNonQuery();
        }

        private PathFinderResult MapDbToModel(SqliteDataReader reader)
        {
            if (string.IsNullOrEmpty(reader["Input"].ToString())) return null;

            return new PathFinderResult
            {
                Input = reader["Input"].ToString(),
                HasValidPath = reader["IsValid"].ToString() == "1",
                MostEfficientPath = reader["Result"].ToString()
            };
        }

        public void Dispose()
        {
            _sqliteConnection?.Dispose();
        }
    }
}