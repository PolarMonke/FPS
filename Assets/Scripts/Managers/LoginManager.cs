using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using UnityEngine.Rendering;
using System.Text.RegularExpressions;

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
                loginInput.text = "";
                passwordInput.text = "";
                loginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "No such user";
                passwordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "No such user";
            }
        }
        else
        {
            loginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Input correct data";
            passwordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Input correct data";
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
            if (newPasswordInput.text == newPasswordConfirmInput.text && Regex.IsMatch(newPasswordInput.text, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                if (!UsersDB.Instance.SearchUser(newLoginInput.text))
                {
                    UsersDB.Instance.AddUser(newLoginInput.text, newPasswordInput.text);
                    LogIn(newLoginInput.text, newPasswordInput.text);
                }
                else
                {
                    newLoginInput.text = "";
                    newLoginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "User already exists";
                }
            }
            else if (!Regex.IsMatch(newPasswordInput.text, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                newPasswordInput.text = "";
                newPasswordInput.text = "";
                newPasswordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Password is too weak";
            }
            else
            {
                newPasswordInput.text = "";
                newPasswordInput.text = "";
                newPasswordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Passwords don't match";
            }
        }
        else
        {
            newLoginInput.text = "";
            newPasswordInput.text = "";
            newPasswordInput.text = "";
            newLoginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Please fill all data";
        }
    }
}
