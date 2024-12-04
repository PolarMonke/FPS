using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;

public class WeaponsDB : MonoBehaviour
{
    public static WeaponsDB Instance { get; set; }
    
    private const string SQL_TABLE_NAME = "Weapons";
    private const string COL_MODEL = "Model";
    private const string COL_AMMO_TYPE = "AmmoType";

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
            dbPath = Application.dataPath + "/DataBases/weapons.sqlite";
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
}
