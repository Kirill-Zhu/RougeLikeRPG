using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Windows;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Zenject;

public class AuthentificationManager : MonoBehaviour
{
    [Inject, SerializeField] CloudSaveManager cloudsaveManager;
   
    [SerializeField] AuthenticationUIController authenticateionUIcontroller;
    [SerializeField] string userName;
    [SerializeField] string password;
    private bool eventsInitialized = false;
    public string WarningMessage;
    public static UnityEvent<string> OnErrorMessage = new UnityEvent<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Event Bus
    EventBinding<OnUserChangeName> OnUserChangeName;
    async void Awake() {
        Application.runInBackground = true;
        await StartClientService();
        Debug.Log("Client service is Run");
        
    }
    private void OnEnable() {
        //Event Bus
        OnUserChangeName = new EventBinding<OnUserChangeName>(newNameClass => userName = newNameClass.newName);
        EventBus<OnUserChangeName>.Register(OnUserChangeName);
    }
    private void OnDisable() {
        //Event Bus
        EventBus<OnUserChangeName>.Deregister(OnUserChangeName);
    }
    private void Start() {

        //Events
       
        authenticateionUIcontroller.OnChangePassword.AddListener(value => password = value);
        authenticateionUIcontroller.OnSignInButtonClicked.AddListener(SignInTest);
        authenticateionUIcontroller.OnSignUpButtonClicked.AddListener(SignUp);
    }

    [ContextMenu("SignIn")]
    public void SignInTest() {
        Debug.Log("Sign In Test");
        SignInWithUsernameAndPaaswordAsycn(userName, password);
    }
    [ContextMenu("Sigh UP")]
    public void SignUp() {
        Debug.Log("Sign Up Test");
        SignUpWithWithUsernameAndPassword(userName, password);

    }
    public async UniTask StartClientService() {
        Debug.Log("Start Client Service");
        try {
            if (UnityServices.State != ServicesInitializationState.Initialized) {

                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                await UnityServices.InitializeAsync();
            }

            if (!eventsInitialized) {
                SetupEvents();
            }
            if (AuthenticationService.Instance.SessionTokenExists) {
               await SighnInAonymouslyAsync();
                Debug.Log($"Session toke ins {AuthenticationService.Instance.SessionToken}");
            } else {

            }

        } catch (Exception exeption) {
            Debug.LogError("Failed to connect");
            OnErrorMessage.Invoke("Failed to connect");
        }
        //Load Player Cloud Data
        cloudsaveManager.LoadData();

        var playerInfo = await AuthenticationService.Instance.GetPlayerInfoAsync();
        string registeredUsername = playerInfo.Username;
        EventBus<OnPlayerSignIn>.Raise(new OnPlayerSignIn { userName = registeredUsername });
        ////------
    }
    public async void SignInWithUsernameAndPaaswordAsycn(string username, string password) {

        try {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        } catch (AuthenticationException ex) {
            OnErrorMessage.Invoke(ex.ToString());
            Debug.LogError(ex);
        } catch (RequestFailedException ex) {
            OnErrorMessage.Invoke(ex.ToString());
            Debug.LogError(ex);
        }
    }
    public async void SignUpWithWithUsernameAndPassword(string username, string password) {
        Debug.Log("Try To sign Up");
        if (!UserNameIsLegit(username)) return;
        if (!PasswordIsLegit(password)) return;


        try {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        } catch (AuthenticationException ex) {
            OnErrorMessage.Invoke(ex.ToString());
            Debug.LogError(ex);
        } catch (RequestFailedException ex) {
            OnErrorMessage.Invoke(ex.ToString());
            Debug.LogError(ex);
        }
    }
    public async UniTask SighnInAonymouslyAsync() {
        try {
           
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

           
           
            Debug.Log($"Player : {AuthenticationService.Instance.PlayerName} is SignedIn");
        } catch {

        }
    }

    public void SignOut() {
        AuthenticationService.Instance.SignOut();
    }
    private void SetupEvents() {
        eventsInitialized = true;
        AuthenticationService.Instance.SignedIn += async () => {
            SignInCOnfirmAsync();
            authenticateionUIcontroller.ClosePage();
             var name = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log($"User Name is {AuthenticationService.Instance.PlayerName}");
        };
        AuthenticationService.Instance.SignedOut += () => {

        };
        AuthenticationService.Instance.Expired += () => {
            SighnInAonymouslyAsync();
        };
    }

    private async void SignInCOnfirmAsync() {
        try {

            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName)) {
               await AuthenticationService.Instance.UpdatePlayerNameAsync("Player");
            }
        } finally {

        }
    }

    bool PasswordIsLegit(string value) {
        if(value.Length < 8) {
            OnErrorMessage.Invoke("Password must be minimum 8 characters");
            return false;
        }
        if (value.Length > 30) {
            OnErrorMessage.Invoke("Password must be maximum 30 characters");
            return false;
        }
        if (!value.Any(char.IsLower)) {
            OnErrorMessage.Invoke("At least one lowercase letter");
            return false;
        }
        if (!value.Any(char.IsUpper)){
            OnErrorMessage.Invoke("At least one uppercase letter");
            return false;
        }

        if (!value.Any(char.IsDigit)) {
            OnErrorMessage.Invoke("At least one digit letter");
            return false;
        }
        if(!Regex.IsMatch(value, @"[*&%$#!]")) {
            OnErrorMessage.Invoke("Symbol: At least one special character/symbol . * & % $ # !");
            return false;
        }
        return true;
    }
    bool UserNameIsLegit(string value) {
        if (value.Length<3 || value.Length>20) {
            OnErrorMessage.Invoke("Name length must be between 3 and 20 characters.");
            return false;
        }
        if(!value.Any(char.IsUpper)|| !value.Any(char.IsLower)) {
            OnErrorMessage.Invoke(" Only letters (A-Z, a-z), numbers (0-9), and the symbols . (period), - (dash), @ (at), and _ (underscore).");
        }
        if (Regex.IsMatch(value, @"[.-@_]")) {
            OnErrorMessage.Invoke(" Only letters (A-Z, a-z), numbers (0-9), and the symbols . (period), - (dash), @ (at), and _ (underscore).");
            return false;
        }
        return true;
    }
}
