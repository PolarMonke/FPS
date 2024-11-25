using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;

public class LanguagesDB : MonoBehaviour
{
    public static LanguagesDB Instance { get; set; }

    public const string SQL_TABLE_NAME = "Belarusian";
    private const string COL_VARIABLE = "Variable";
    private const string COL_TEXT = "Text";

    private string dbPath;
    private IDbConnection connection = null;

    public Dictionary<string, string> languageData = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
            dbPath = Application.dataPath + "/DataBases/Languages.sqlite";
            if (!File.Exists(dbPath))
            {
                Debug.LogError("Database file not found at: " + dbPath);
                return;
            }
            OpenConnection();
            LoadLanguageData();
        }
    }
    private bool OpenConnection()
    {
        string connectionString = $"URI=file:{dbPath}";
        connection = new SqliteConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Languages Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening weapon database connection: {e.Message}");
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

        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT {COL_VARIABLE}, {COL_TEXT} FROM {SQL_TABLE_NAME}";

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string variable = reader.GetString(0);
                    string text = reader.GetString(1);
                    languageData.Add(variable, text);
                }
            }
        }

        Debug.Log("Language data loaded successfully!");
    }


    public string GetText(string key)
    {
        if (languageData.ContainsKey(key))
        {
            return languageData[key];
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in language data!");
            return "";
        }
    }
}
