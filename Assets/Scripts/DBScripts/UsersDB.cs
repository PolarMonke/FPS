using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;

public class UsersDB : MonoBehaviour
{
    public static UsersDB Instance { get; set; }

    private const string SQL_TABLE_NAME = "Users";
    private const string COL_USERNAME = "Username";
    private const string COL_PASSWORD = "Password";

    private string dbPath;
    private IDbConnection connection = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
            dbPath = Application.dataPath + "/DataBases/Users.sqlite";
            if (!File.Exists(dbPath))
            {
                Debug.LogError("Database file not found at: " + dbPath);
                return;
            }
            OpenConnection();
        }
        DontDestroyOnLoad(this);
    }
    private bool OpenConnection()
    {
        string connectionString = $"URI=file:{dbPath}";
        connection = new SqliteConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Users Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening weapon database connection: {e.Message}");
            return false;
        }
    }

    public bool CompareData(string username, string password)
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT * FROM {SQL_TABLE_NAME} WHERE {COL_USERNAME} = @username AND {COL_PASSWORD} = @password";
            command.Parameters.Add(new SqliteParameter("@username", username));
            command.Parameters.Add(new SqliteParameter("@password", password));
            using (IDataReader reader = command.ExecuteReader())
            {
                return reader.Read();
            }
        }
    }
    public void AddUser(string username, string password)
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO {SQL_TABLE_NAME} ({COL_USERNAME}, {COL_PASSWORD}) VALUES (@username, @password)";
            command.Parameters.Add(new SqliteParameter("@username", username));
            command.Parameters.Add(new SqliteParameter("@password", password));
            command.ExecuteNonQuery();
        }
    }
    public void DeleteUser(string username)
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM {SQL_TABLE_NAME} WHERE {COL_USERNAME} = @username";
            command.Parameters.Add(new SqliteParameter("@username", username));
            command.ExecuteNonQuery();
        }
    }

    public bool SearchUser(string username)
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT * FROM {SQL_TABLE_NAME} WHERE {COL_USERNAME} = @username";
            command.Parameters.Add(new SqliteParameter("@username", username));
            using (IDataReader reader = command.ExecuteReader())
            {
                return reader.Read();
            }
        }
    }
}