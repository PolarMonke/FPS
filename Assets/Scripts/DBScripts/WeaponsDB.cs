using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;
using UnityEditor;

public class WeaponsDB : MonoBehaviour
{
    public static WeaponsDB Instance { get; set; }
    
    private const string SQL_TABLE_NAME = "Weapons";
    private const string COL_MODEL = "Model";
    private const string COL_AMMO_TYPE = "AmmoType";
    private const string COL_SPRITE = "Sprite";

    private string dbName = "Weapons1.sqlite";
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
            DontDestroyOnLoad(gameObject);

            dbPath = Path.Combine(Application.persistentDataPath, "DataBases", dbName);
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            if (!File.Exists(dbPath))
            {
                CopyDatabaseFromStreamingAssets();
            }
            OpenConnection();
        }
       DontDestroyOnLoad(this);
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
        connection = new SqliteConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Weapon Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening weapon database connection: {e.Message}");
            return false;
        }
    }

    public string GetAmmoTypeByWeapon(string weaponModel)
    {
        string ammoType = null;
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return null;
        }
        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_AMMO_TYPE} FROM {SQL_TABLE_NAME} WHERE {COL_MODEL} = '{weaponModel}'";
                using (IDataReader reader = command.ExecuteReader())
                {
                if (reader.Read())
                {
                    ammoType = reader.GetString(0);
                }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetAmmoTypeByWeapon error: " + e.Message);
        }
        return ammoType;
    }

    public List<string> GetAllWeaponModels()
    {
        List<string> weaponModels = new List<string>();
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return weaponModels; 
        }

        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_MODEL} FROM {SQL_TABLE_NAME}"; // Simplified query
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) // Read all rows
                    {
                        weaponModels.Add(reader.GetString(0));
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetAllAmmoModels error: " + e.Message);
        }

        return weaponModels;
    }

    public Sprite GetSpriteByName(string name)
    {
        Sprite weaponSprite = null;
        string spritePath = "";
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return null;
        }

        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_SPRITE} FROM {SQL_TABLE_NAME} WHERE {COL_MODEL} = '{name}'";
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

        weaponSprite = Resources.Load<Sprite>("Textures/WeaponSprites/" + spritePath);

        return weaponSprite;
    }
}
