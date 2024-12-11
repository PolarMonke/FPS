using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using System;

public class ChractersDB : MonoBehaviour
{
    public static ChractersDB Instance { get; set; }
    
    private string SQL_TABLE_NAME = "Characters";
    private const string COL_ID = "ID";
    private const string COL_NAME = "Name";
    private const string COL_WEAPON = "Weapon";
    private const string COL_BONUS = "Bonus";
    private const string COL_OWNER = "Owner";

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
                command.CommandText = $"SELECT {COL_ID}, {COL_NAME}, {COL_WEAPON}, {COL_BONUS} FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = NULL";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = Int32.Parse(reader.GetString(0));
                        string name = reader.GetString(1);
                        string weapon = reader.GetString(2);
                        string bonus = reader.GetString(3);
                        string owner = reader.GetString(4);
                        characterDatas.Add(new CharacterData(ID, name, weapon, bonus, owner));
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

    public List<CharacterData> LoadCharacters(string username)
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
                command.CommandText = $"SELECT {COL_ID}, {COL_NAME}, {COL_WEAPON}, {COL_BONUS}, {COL_OWNER} FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = {username}";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID = Int32.Parse(reader.GetString(0));
                        string name = reader.GetString(1);
                        string weapon = reader.GetString(2);
                        string bonus = reader.GetString(3);
                        string owner = reader.GetString(4);
                        characterDatas.Add(new CharacterData(ID, name, weapon, bonus, owner));
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
            command.CommandText = $"INSERT INTO {SQL_TABLE_NAME} ({COL_ID}, {COL_NAME}, {COL_WEAPON}, {COL_BONUS}, {COL_OWNER}) VALUES (@ID, @Name, @Weapon, @Bonus, @Owner)";
            command.Parameters.Add(new SqliteParameter("@ID", character.ID));
            command.Parameters.Add(new SqliteParameter("@Name", character.Name));
            command.Parameters.Add(new SqliteParameter("@Weapon", character.WeaponModel));
            command.Parameters.Add(new SqliteParameter("@Bonus", character.BonusType));
            command.Parameters.Add(new SqliteParameter("@Owner", character.Owner));
            command.ExecuteNonQuery();
        }
    }
    public void DeleteCharacter(int id)
    {
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return;
        }
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = {id}";
            command.ExecuteNonQuery();
        }
        RenumberIDs();
    }

    public void DeleteUser(string username)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM {SQL_TABLE_NAME} WHERE {COL_OWNER} = {username}";
            command.ExecuteNonQuery();
        }
        RenumberIDs();
    }


    private void RenumberIDs() {
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return;
        }
        try {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT ID FROM {SQL_TABLE_NAME}";
                using (var reader = command.ExecuteReader())
                {
                    int newID = 1;
                    while (reader.Read())
                    {
                        int oldID = reader.GetInt32(0);
                        command.CommandText = $"UPDATE {SQL_TABLE_NAME} SET ID = {newID} WHERE ID = {oldID}";
                        command.ExecuteNonQuery();
                        newID++;
                    }
                }
            }
        } 
        catch (Exception e) {
            Debug.LogError($"RenumberIDs error: {e.Message}");
        }

    }

    public int GetLastID()
    {
        int lastID = 0;
        if (connection == null || connection.State != ConnectionState.Open)
        {
            if (!OpenConnection()) return 0;
        }
        try
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT MAX({COL_ID}) FROM {SQL_TABLE_NAME}";
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    lastID = Convert.ToInt32(result);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetLastID error: " + e.Message);
        }
        return lastID;
    }

}