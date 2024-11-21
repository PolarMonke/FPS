using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.Data;

public class WeaponsDB : MonoBehaviour
{
    public static WeaponsDB Instance { get; set; }

    private const string SQL_DB_NAME = "Weapons";
    private const string SQL_TABLE_NAME = "Weapons";
    private const string COL_MODEL = "Model";
    private const string COL_AMMO_TYPE = "AmmoType";

    private IDbConnection _connection = null;
	private IDbCommand _command = null;
	private IDataReader _reader = null;
	private string _sqlString;

		public bool _createNewTable = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }
}
