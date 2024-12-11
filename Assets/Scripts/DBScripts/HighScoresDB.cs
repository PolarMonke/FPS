using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;

public class HighScoreDB : MonoBehaviour
{
    public static HighScoreDB Instance { get; set; }
    
    private string SQL_TABLE_NAME = "HighScores";
    private const string COL_OWNER = "Owner";
    private const string COL_SCORE = "HighScore";

    private string dbPath;
    private IDbConnection connection = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            dbPath = Application.dataPath + "/DataBases/HighScores.sqlite";
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
            Debug.Log("High scores Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening bonuses database connection: {e.Message}");
            return false;
        }
    }

    public int LoadUserScore(string username)
    {
        string query = $"SELECT {COL_SCORE} FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = @username";
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@username", username));

            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public void SaveHighScore(string username, int score)
    {
        string query = $"SELECT COUNT(*) FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = @username";
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@username", username));
            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count > 0)
            {
                query = $"UPDATE {SQL_TABLE_NAME} SET {COL_SCORE} = @score WHERE {COL_OWNER} = @username";
            }
            else
            {
                query = $"INSERT INTO {SQL_TABLE_NAME} ({COL_OWNER}, {COL_SCORE}) VALUES (@username, @score)";
            }

            command.CommandText = query;
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("@username", username));
            command.Parameters.Add(new SqliteParameter("@score", score));

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving score: {e.Message}");
            }
        }
    }
    public bool UserHasRecords(string username)
    {
        string query = $"SELECT COUNT(*) FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = @username";
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@username", username));

            try
            {
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error checking user records: {e.Message}");
                return false;
            }
        }
    }
}