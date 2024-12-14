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
    public GameObject accountMenu;
    public GameObject accountInfoMenu;
    public GameObject loginMenu;
    public GameObject registrationMenu;

    [Header("Log in menu")]
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button newAccountButton;

    public TextMeshProUGUI loginErrorText;
    public TextMeshProUGUI passwordErrorText;

    [Header("New account menu")]
    public TMP_InputField newLoginInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField newPasswordConfirmInput;
    public Button createNewAccountButton;

    public TextMeshProUGUI newLoginErrorText;
    public TextMeshProUGUI newPasswordErrorText;

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

    private void Start()
    {
        loginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("EnterLogin");
        passwordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("EnterPassword");
        loginButton.GetComponentInChildren<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("LogIn");
        newAccountButton.GetComponentInChildren<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("CreateNewAccount");

        newLoginInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("EnterLogin");
        newPasswordInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("EnterPassword");
        newPasswordConfirmInput.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("RepeatPassword");
        createNewAccountButton.GetComponentInChildren<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("Registrate");
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
        if (AccountManager.Instance.isLogged)
        {
            accountInfoMenu.SetActive(true);
        }
        else
        {
            loginMenu.SetActive(true);
        }
    }
    public void ExitAccountInfoMenu()
    {
        accountInfoMenu.SetActive(false);
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
                loginErrorText.text = "";
                passwordErrorText.text = "";
                AccountManager.Instance.LogIn(loginInput.text);
            }
            else
            {
                loginInput.text = "";
                passwordInput.text = "";
                loginErrorText.text = LanguagesDB.Instance.GetText("NoUserException");
            }
        }
        else
        {
            passwordErrorText.text = LanguagesDB.Instance.GetText("WrongDataException");
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
                    newLoginErrorText.text = "";
                    newPasswordErrorText.text = "";
                    UsersDB.Instance.AddUser(newLoginInput.text, newPasswordInput.text);
                    LogIn(newLoginInput.text, newPasswordInput.text);
                }
                else
                {
                    newLoginInput.text = "";
                    newLoginErrorText.text = LanguagesDB.Instance.GetText("UserExistsException");
                }
            }
            else if (!Regex.IsMatch(newPasswordInput.text, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                newPasswordInput.text = "";
                newPasswordConfirmInput.text = "";
                newPasswordErrorText.text = LanguagesDB.Instance.GetText("WeakPasswordException");
            }
            else
            {
                newPasswordInput.text = "";
                newPasswordConfirmInput.text = "";
                newPasswordErrorText.text = LanguagesDB.Instance.GetText("DifferentPasswordsException");
            }
        }
        else
        {
            newPasswordErrorText.text = LanguagesDB.Instance.GetText("WrongDataException");
        }
    }
}
