using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GuestCreator : MonoBehaviour
{
    public static GuestCreator instance;

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

    public void CreateGuestProfile(System.Action<bool> callback)
    {
        retryCallback = callback;
        string guestCustomID = SystemInfo.deviceUniqueIdentifier;

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = guestCustomID,
            CreateAccount = true
        }, result =>
        {
            Debug.Log("✅ Guest profile successfully created!");
            SaveGuestInfo(callback);
        }, error =>
        {
            Debug.LogError("❌ Failed to create guest profile: " + error.GenerateErrorReport());
            ShowError("Guest account creation failed. Please try again.");
            callback?.Invoke(false);
        });
    }

    private void SaveGuestInfo(System.Action<bool> callback)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { "hasPlayedBefore", "true" } }
        }, success =>
        {
            Debug.Log("✅ Guest profile data saved successfully!");
            SetInitialXP(callback); // Meghívjuk az XP beállítását
        }, error =>
        {
            Debug.LogError("❌ Error saving guest profile data: " + error.GenerateErrorReport());
            ShowError("Error saving account data. Please retry.");
            callback?.Invoke(false);
        });
    }

    private void SetInitialXP(System.Action<bool> callback)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "setInitialXP" // A CloudScript függvény neve
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            Debug.Log("✅ XP successfully initialized!");
            callback?.Invoke(true);
        }, error =>
        {
            Debug.LogError("❌ Failed to set initial XP: " + error.GenerateErrorReport());
            ShowError("Failed to set XP. Please retry.");
            callback?.Invoke(false);
        });
    }

    private void ShowError(string message)
    {
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RetryGuestCreation);

        lastErrorMessage = message;
        errorPanel.SetActive(true);
        retryButton.gameObject.SetActive(true);
        if (errorText != null) errorText.text = message;
    }

    public void RetryGuestCreation()
    {
        Debug.Log("🔄 Retrying guest account creation...");
        errorPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
        CreateGuestProfile(retryCallback);
    }
}
