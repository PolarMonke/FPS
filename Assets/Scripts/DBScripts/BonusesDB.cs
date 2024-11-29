using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;

public class BonusesDB : MonoBehaviour
{
    public static BonusesDB Instance { get; set; }
    
    private const string SQL_TABLE_NAME = "BonusesBel";
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

}
