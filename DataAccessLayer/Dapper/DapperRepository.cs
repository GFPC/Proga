using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Model;

namespace DataAccessLayer.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository(string dbPath = "app.db")
        {
            _connectionString = $"Data Source={dbPath}";
            EnsureTableCreated();
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        private void EnsureTableCreated()
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            if (typeof(T) == typeof(Student))
            {
                string sql = $@"
                    CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Speciality TEXT NOT NULL,
                        [Group] TEXT NOT NULL
                    );";
                connection.Execute(sql);
            }
        }

        private string GetTableName()
        {
            var name = typeof(T).Name;
            return name.EndsWith("s") ? name : name + "s";
        }

        public void Add(T entity)
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            if (entity is Student student)
            {
                string sql = $"INSERT INTO {tableName} (Name, Speciality, [Group]) VALUES (@Name, @Speciality, @Group); SELECT last_insert_rowid();";
                int newId = connection.QuerySingle<int>(sql, new { student.Name, student.Speciality, student.Group });
                student.Id = newId;
            }
        }

        public void Delete(int id)
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            string sql = $"DELETE FROM {tableName} WHERE Id = @Id;";
            connection.Execute(sql, new { Id = id });
        }

        public void Update(T entity)
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            if (entity is Student student)
            {
                string sql = $"UPDATE {tableName} SET Name = @Name, Speciality = @Speciality, [Group] = @Group WHERE Id = @Id;";
                connection.Execute(sql, new { student.Name, student.Speciality, student.Group, student.Id });
            }
        }

        public T? ReadById(int id)
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            string sql = $"SELECT * FROM {tableName} WHERE Id = @Id;";
            return connection.QueryFirstOrDefault<T>(sql, new { Id = id });
        }

        public List<T> ReadAll()
        {
            using var connection = CreateConnection();
            string tableName = GetTableName();
            string sql = $"SELECT * FROM {tableName};";
            return connection.Query<T>(sql).ToList();
        }
    }

    public class DapperRepository : DapperRepository<Student>, IRepository
    {
        public DapperRepository(string dbPath = "app.db") : base(dbPath) { }
    }
}
