using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; set; }

    
    
    public bool isLogged;
    public string username;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    
    public void LogIn(string username)
    {
        this.username = username;
        isLogged = true;
        LoginManager.Instance.ExitLogin();
        LoginManager.Instance.ExitRegistration();
        LoginManager.Instance.EnterAccountInfoMenu();
        LoginManager.Instance.accountInfoMenu.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = username;
    }
    public void LogOut()
    {
        isLogged = false;
        LoginManager.Instance.ExitAccountInfoMenu();
        LoginManager.Instance.EnterLogin();
    }
    public void DeleteAccount()
    {
        UsersDB.Instance.DeleteUser(username);
        SaveLoadManager.Instance.DeleteUserSaveData(username);
        UsersDB.Instance.DeleteUser(username);
        LoginManager.Instance.ExitAccountInfoMenu();
        LoginManager.Instance.EnterLogin();     
    }
}
