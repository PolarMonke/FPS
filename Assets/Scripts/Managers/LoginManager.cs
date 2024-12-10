using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using UnityEngine.Rendering;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; set; }

    [Header("Menus")]
    public GameObject loginMenu;
    public GameObject registrationMenu;

    [Header("Log in menu")]
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button newAccountButton;

    [Header("New account menu")]
    public TMP_InputField newLoginInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField newPasswordConfirmInput;
    public Button createNewAccountButton;


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
    }

    public void EnterLogin()
    {
        loginMenu.SetActive(true);
    }
    public void ExitLogin()
    {
        loginMenu.SetActive(false);
    }
    public void EnterRegistration()
    {
        registrationMenu.SetActive(true);
    }
    public void ExitRegistration()
    {
        registrationMenu.SetActive(false);
    }

    public void LogIn()
    {
        if (loginInput.text != "" && passwordInput.text != "")
        {
            if (UsersDB.Instance.CompareData(loginInput.text, passwordInput.text))
            {
                AccountManager.Instance.LogIn(loginInput.text);
            }
            else
            {
                //inform user
            }
        }
        else
        {
            //inform user
        }
    }
    public void LogIn(string login, string password)
    {
        if (UsersDB.Instance.CompareData(login, password))
        {
            AccountManager.Instance.LogIn(login);
        }
    }
    public void Registrate()
    {
        if (newLoginInput.text != "" && newPasswordInput.text != "" && newPasswordConfirmInput.text != "")
        {
            if (newPasswordInput.text == newPasswordConfirmInput.text)
            {
                if (!UsersDB.Instance.SearchUser(newLoginInput.text))
                {
                    UsersDB.Instance.AddUser(newLoginInput.text, newPasswordInput.text);
                    LogIn(newLoginInput.text, newPasswordInput.text);
                }
                else
                {
                    //inform user
                }
            }
            else
            {
                //inform user
            }
        }
        else
        {
            //inform user
        }
    }
}
