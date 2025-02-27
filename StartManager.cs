using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public static StartManager instance;

    IEnumerator DelayedStartGame()
    {
        yield return new WaitForSeconds(0.5f);
        StartGame();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(DelayedStartGame());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void StartGame()
    {
        if (InternetChecker.instance == null || LoginManager.instance == null || GuestCreator.instance == null)
        {
            Debug.LogError("❌ Egyik Manager sem példányosított megfelelően!");
            return;
        }

        Debug.Log("🔹 Internet ellenőrzése...");
        InternetChecker.instance.CheckInternetConnection(isConnected =>
        {
            if (!isConnected) return;

            Debug.Log("🔹 Bejelentkezés...");
            LoginManager.instance.LogIn(success =>
            {
                if (success)
                {
                    Debug.Log("✅ Bejelentkezés sikeres, adatok lekérése...");
                    FetchUserData();
                }
                else
                {
                    Debug.LogError("❌ Bejelentkezési hiba!");
                }
            });
        });
    }

    void FetchUserData()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogError("❌ Nincs bejelentkezve, nem lehet adatokat lekérni.");
            return;
        }

        Debug.Log("🔹 Felhasználói adatok lekérése...");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("hasPlayedBefore"))
            {
                Debug.Log("✅ A játékos már játszott korábban.");
                LoadScene("MenuScene");
            }
            else
            {
                Debug.Log("🔹 Első játék, vendégfiók létrehozása...");
                GuestCreator.instance.CreateGuestProfile(isSuccess =>
                {
                    if (isSuccess)
                    {
                        Debug.Log("✅ Vendégfiók sikeresen létrehozva!");
                        LoadScene("TutorialScene");
                    }
                });
            }
        },
        error =>
        {
            Debug.LogError("❌ Hiba a felhasználói adatok lekérésekor: " + error.GenerateErrorReport());
        });
    }

    void LoadScene(string sceneName)
    {
        Debug.Log($"🔹 {sceneName} nevű jelenet betöltése...");
        SceneManager.LoadScene(sceneName);
    }
}
