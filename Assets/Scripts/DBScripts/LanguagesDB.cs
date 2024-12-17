using System;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;

public class LanguagesDB : MonoBehaviour
{
    public static LanguagesDB Instance { get; set; }

    public string SQL_TABLE_NAME = "Belarusian";
    private const string COL_VARIABLE = "Variable";
    private const string COL_TEXT = "Text";

    private string dbName = "Languages1.sqlite";
    private string dbPath;
    private IDbConnection connection = null;

    public Dictionary<string, string> languageData = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dbPath = Path.Combine(Application.persistentDataPath, "DataBases", dbName);
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

        if (!File.Exists(dbPath))
        {
            CopyDatabaseFromStreamingAssets();
        }

        if (!OpenConnection()) {
            Debug.LogError("Failed to open database connection after copy attempt.");
        } else {
            LoadLanguageData();
        }
    }

    private void CopyDatabaseFromStreamingAssets()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "DataBases", dbName);

        if (!File.Exists(sourcePath))
        {
            Debug.LogError($"Database file not found in StreamingAssets: {sourcePath}");
            return;
        }

        try
        {
            File.Copy(sourcePath, dbPath, true);
            Debug.Log($"Database copied to: {dbPath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Error copying database: {e.Message}");

        }
    }

    private bool OpenConnection()
    {
        string connectionString = $"URI=file:{dbPath}";
        Debug.Log($"Connection String: {connectionString}");

        connection = new SqliteConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Database connection opened successfully.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error opening database connection: {e.Message}");
            if (e is FileNotFoundException) {
                Debug.LogError("FileNotFoundException.  The file specified in the connection string may not exist.");
            }
            if (e is IOException) {
                Debug.LogError("IOException.  There may be a problem with file access permissions or file locking.");
            }

            return false;
        }
    }

    public void LoadLanguageData()
    {
        if (connection == null || connection.State != ConnectionState.Open)
        {
            Debug.LogError("Database connection is not open!");
            return;
        }

        languageData.Clear();
        int rowsRead = 0;

        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT {COL_VARIABLE}, {COL_TEXT} FROM {SQL_TABLE_NAME}";
            Debug.Log($"Executing SQL query: {command.CommandText}");


            try
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try {
                            string variable = reader.GetString(0);
                            string text = reader.GetString(1);
                            languageData.Add(variable, text);
                            rowsRead++;
                        } catch (InvalidCastException e) {
                            Debug.LogError($"Error reading database row: {e.Message}");
                        }
                    }
                }

                if (rowsRead > 0) {
                    Debug.Log($"Language data loaded successfully! {rowsRead} rows read.");
                } else {
                    Debug.LogWarning("No language data found in the database.");
                }
            }
            catch (Exception ex) {
                Debug.LogError($"Unexpected error: {ex.Message}");
            }

        }
    }

    public string GetText(string key)
    {
        if (languageData.ContainsKey(key))
        {
            return languageData[key];
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in language data! Check your database for this entry.");
            return "";
        }
    }

}
