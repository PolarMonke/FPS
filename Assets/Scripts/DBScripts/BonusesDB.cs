using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;
using UnityEditor;


public class BonusesDB : MonoBehaviour
{
    public static BonusesDB Instance { get; set; }
    
    public string SQL_TABLE_NAME = "BonusesBel";
    private const string COL_NAME = "Name";
    private const string COL_DESCRIPTION = "Description";
    private const string COL_IMAGE = "Image";
    private const string COL_DURATION = "Duration";
    private const string COL_UNITED_NAME = "UnitedName";

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
            dbPath = Application.dataPath + "/DataBases/Bonuses.sqlite";
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
            Debug.Log("Bonuses Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening bonuses database connection: {e.Message}");
            return false;
        }
    }

    public (string, string, string, int) GetBonusByName(string name)
    {
        string bonusName = "";
        string bonusDesc = "";
        string bonusSprite = "";
        int bonusDuration = 0;
        
        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_NAME} FROM {SQL_TABLE_NAME} WHERE {COL_UNITED_NAME} = '{name}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonusName = reader.GetString(0);
                    }
                }
                command.CommandText = $"SELECT {COL_DESCRIPTION} FROM {SQL_TABLE_NAME} WHERE {COL_UNITED_NAME} = '{name}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonusDesc = reader.GetString(0);
                    }
                }
                command.CommandText = $"SELECT {COL_IMAGE} FROM {SQL_TABLE_NAME} WHERE {COL_UNITED_NAME} = '{name}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonusSprite = reader.GetString(0);
                    }
                }
                command.CommandText = $"SELECT {COL_DURATION} FROM {SQL_TABLE_NAME} WHERE {COL_UNITED_NAME} = '{name}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonusDuration = Int32.Parse(reader.GetString(0));
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetAmmoTypeByWeapon error: " + e.Message);
        }

        return (bonusName, bonusDesc, bonusSprite, bonusDuration);
    }

    public Dictionary<string, string> GetAllBonusNames()
    {
        Dictionary<string, string> bonusNames = new Dictionary<string, string>();
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return bonusNames;
        }

        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_UNITED_NAME}, {COL_NAME} FROM {SQL_TABLE_NAME}";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string unitedName = reader.GetString(0);
                        string bonusName = reader.GetString(1);
                        bonusNames.Add(unitedName, bonusName);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetAllBonusNames error: " + e.Message);
        }

        return bonusNames;
    }
    
    public Sprite GetSpriteByName(string name)
    {
        Sprite bonusSprite = null;
        string spritePath = "";
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return null;
        }

        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_IMAGE} FROM {SQL_TABLE_NAME} WHERE {COL_UNITED_NAME} = '{name}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        spritePath = reader.GetString(0);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetAllBonusNames error: " + e.Message);
        }

        bonusSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/Bonuses/" + spritePath);

        return bonusSprite;
    }
}
