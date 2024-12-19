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
    
}
