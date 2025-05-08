using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ConnectionMonitor : MonoBehaviour
{
    public static ConnectionMonitor instance;

    public GameObject errorPanel; // Internet hiba UI panel
    public Button retryButton; // Újrapróbálkozás gomb
    public TextMeshProUGUI errorText; // Hibaüzenet szövege
    private bool isChecking = false; // Ne fusson több ellenőrzés egyszerre

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

    void Start()
    {
        StartCoroutine(InternetCheckLoop());

        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => CheckInternetConnection());
        }
    }

    IEnumerator InternetCheckLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // 5 másodpercenként ellenőrzi a kapcsolatot

            if (!isChecking) // Ne indítson új ellenőrzést, ha már fut egy
            {
                StartCoroutine(CheckInternetConnection());
            }
        }
    }

    IEnumerator CheckInternetConnection()
    {
        isChecking = true;
        using (var request = new UnityEngine.Networking.UnityWebRequest("https://clients3.google.com/generate_204"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                //Debug.Log("✅ Internetkapcsolat aktív.");
                errorPanel.SetActive(false);
            }
            else
            {
                //Debug.LogError("❌ Internetkapcsolat megszakadt!");
                ShowError("Connection lost. Please check your internet and try again.");
            }
        }
        isChecking = false;
    }

    private void ShowError(string message)
    {
        errorPanel.SetActive(true);
        if (errorText != null) errorText.text = message;
    }
}
