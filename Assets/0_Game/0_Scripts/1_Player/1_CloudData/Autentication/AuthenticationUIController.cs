using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class AuthenticationUIController : MonoBehaviour {
    [SerializeField] UIDocument authenticationUIDocument;

    const string AuthenticationMenu = "AuthenticationMenu";
    const string SignIn = "SignIn";
    const string SignOut = "SignOut";
    const string SignUp = "SignUp";
    const string Confim = "Confim";
    const string InputNameField = "InputNameField";
    const string InputPasswordField = "InputPasswordField";
    const string ErrorMessage = "ErrorMessage";
    [HideInInspector] public UnityEvent<string> OnChangePassword = new UnityEvent<string>();
    [HideInInspector] public UnityEvent OnSignUpButtonClicked = new UnityEvent();
    [HideInInspector] public UnityEvent OnSignInButtonClicked = new UnityEvent();
    [HideInInspector] public UnityEvent OnConfimButtonClikded = new UnityEvent();
    Label errorMessageLabel;
    //Event BUs
  

    private void Awake() {
        Initialize();
    }
    private void OnEnable() {
        
        if(AuthenticationService.Instance.IsSignedIn)
            ClosePage();
    }
  

    void Initialize() {
        var signInButton = authenticationUIDocument.rootVisualElement.Q<Button>(SignIn);
        var signUpButton = authenticationUIDocument.rootVisualElement.Q<Button>(SignUp);
        var sighOutButton = authenticationUIDocument.rootVisualElement.Q<Button>(SignOut);
        var confimButton = authenticationUIDocument.rootVisualElement.Q<Button>(Confim);

        var inputName = authenticationUIDocument.rootVisualElement.Q<TextField>(InputNameField);
        var inputPassword = authenticationUIDocument.rootVisualElement.Q<TextField>(InputPasswordField);

        errorMessageLabel = authenticationUIDocument.rootVisualElement.Q<Label>(ErrorMessage);

        //Events
        inputName.RegisterValueChangedCallback(OnNameChanged);
        inputPassword.RegisterValueChangedCallback(OnPasswordChanged);
        signUpButton.clicked += SignUpVoid;
        signInButton.clicked += SignInVoid;

        //confimButton.clicked += ConfimChanges;
    }
    void OnNameChanged(ChangeEvent<string> @event) {
        EventBus<OnUserChangeName>.Raise(new OnUserChangeName { newName = @event.newValue });
        Debug.Log("Name Changed");
    }
    void OnPasswordChanged(ChangeEvent<string> @event) {
        OnChangePassword.Invoke(@event.newValue);
    }
    void SignUpVoid() {
        OnSignUpButtonClicked.Invoke();
    }
    void SignInVoid() {
        OnSignInButtonClicked.Invoke();
        Debug.Log("Sign in");
    }

    void ConfimChanges() {
        OnConfimButtonClikded.Invoke();
        Debug.Log("Confim");
    }
    public void OpenPage() {
        authenticationUIDocument.rootVisualElement.Q<VisualElement>(AuthenticationMenu).style.display = DisplayStyle.Flex;
    }
    public void ClosePage() {
        authenticationUIDocument.rootVisualElement.Q<VisualElement>(AuthenticationMenu).style.display = DisplayStyle.None;
    }
    public void ShowError(string errorMsg) {
        errorMessageLabel.text = errorMsg;
    }
}
