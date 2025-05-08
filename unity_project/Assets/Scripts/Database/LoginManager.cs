using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;

    public GameObject errorPanel;
    public Button retryButton;
    public TextMeshProUGUI errorText;
    private System.Action<bool> retryCallback;

    private string lastErrorMessage = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogIn(System.Action<bool> onLoginComplete)
    {
        retryCallback = onLoginComplete;
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        },
        result =>
        {
            Debug.Log("✅ Bejelentkezés sikeres!");
            onLoginComplete?.Invoke(true);
        },
        error =>
        {
            Debug.LogError("❌ Bejelentkezési hiba: " + error.GenerateErrorReport());
            ShowError("Login failed. Please try again.");
            onLoginComplete?.Invoke(false);
        });
    }

    private void ShowError(string message)
    {
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RetryLogin);

        lastErrorMessage = message;
        errorPanel.SetActive(true);
        retryButton.gameObject.SetActive(true);
        if (errorText != null) errorText.text = message;
    }

    public void RetryLogin()
    {
        Debug.Log("🔄 Újrapróbálkozás bejelentkezéssel...");
        errorPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
        LogIn(retryCallback); // **Folytatja az eredeti folyamatot**
    }
}
