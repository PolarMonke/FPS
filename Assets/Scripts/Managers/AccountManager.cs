using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; set; }

    public GameObject accountMenu;
    public GameObject accountInfoMenu;
    
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
    public void EnterAccoutMenu()
    {
        accountMenu.SetActive(true);
        EnterAccountInfoMenu();
    }
    public void ExitAccoutMenu()
    {
        accountMenu.SetActive(false);
    }

    public void EnterAccountInfoMenu()
    {
        accountMenu.SetActive(true);
        if (isLogged)
        {
            accountInfoMenu.SetActive(true);
        }
        else
        {
            LoginManager.Instance.loginMenu.SetActive(true);
        }
    }
    public void ExitAccountInfoMenu()
    {
        accountInfoMenu.SetActive(false);
    }
    public void LogIn(string username)
    {
        this.username = username;
        isLogged = true;
        LoginManager.Instance.ExitLogin();
        LoginManager.Instance.ExitRegistration();
        EnterAccountInfoMenu();
        accountInfoMenu.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = username;
    }
    public void LogOut()
    {
        isLogged = false;
        ExitAccountInfoMenu();
        LoginManager.Instance.EnterLogin();
    }
    public void DeleteAccount()
    {
        username = accountInfoMenu.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        UsersDB.Instance.DeleteUser(username);
        ExitAccountInfoMenu();
        LoginManager.Instance.EnterLogin();     
    }
}
