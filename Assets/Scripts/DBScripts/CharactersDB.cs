using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;

public class CahractersDB : MonoBehaviour
{
    public static CahractersDB Instance { get; set; }
    
    public string SQL_TABLE_NAME = "Characters";
    private const string COL_ID = "ID";
    private const string COL_NAME = "Name";
    private const string COL_WEAPON = "Weapon";
    private const string COL_BONUS = "Bonus";

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
            dbPath = Application.dataPath + "/DataBases/Characters.sqlite";
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
            Debug.Log("Characters Database connection opened successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening bonuses database connection: {e.Message}");
            return false;
        }
    }

    public List<CharacterData> LoadCharacters()
    {
        List<CharacterData> characterDatas = new List<CharacterData>();
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return null;
        }
        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {COL_ID}, {COL_NAME}, {COL_WEAPON}, {COL_BONUS} FROM {SQL_TABLE_NAME}";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = Int32.Parse(reader.GetString(0));
                        string name = reader.GetString(1);
                        string weapon = reader.GetString(2);
                        string bonus = reader.GetString(3);
                        characterDatas.Add(new CharacterData(ID, name, weapon, bonus));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetAllBonusNames error: " + e.Message);
        }

        return characterDatas;
    }

    public void SaveCharacter(CharacterData character)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO {SQL_TABLE_NAME} ({COL_ID}, {COL_NAME}, {COL_WEAPON}, {COL_BONUS}) VALUES ({character.ID}, {character.Name}, {character.WeaponModel}, {character.BonusType})";
            command.ExecuteNonQuery();
        }
    }
    public void DeleteCharacter(int id)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM {SQL_TABLE_NAME} WHERE ID = {id}";
            command.ExecuteNonQuery();
        }
    }
}