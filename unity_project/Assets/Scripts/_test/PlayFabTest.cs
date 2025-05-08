using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabTest : MonoBehaviour
{
    void Start()
    {
        PlayFabLogin();
    }

    void PlayFabLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("✅ PlayFab login successful! ID: " + result.PlayFabId);
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("❌ PlayFab login failed: " + error.GenerateErrorReport());
    }
}
