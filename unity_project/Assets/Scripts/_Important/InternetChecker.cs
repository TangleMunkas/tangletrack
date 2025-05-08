using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InternetChecker : MonoBehaviour
{
    public static InternetChecker instance;

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
            Debug.LogWarning("⚠ InternetChecker példány már létezik, törlés...");
            Destroy(gameObject);
        }
    }

    public void CheckInternetConnection(System.Action<bool> callback)
    {
        retryCallback = callback; // Elmentjük a callback-et az újrapróbálkozásra
        StartCoroutine(CheckInternetCoroutine(callback));
    }

    private IEnumerator CheckInternetCoroutine(System.Action<bool> callback)
    {
        using (var request = new UnityEngine.Networking.UnityWebRequest("https://clients3.google.com/generate_204"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Internetkapcsolat elérhető!");
                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError("❌ Nincs internetkapcsolat!");
                ShowError("No internet connection. Please check your network and try again.");
                callback?.Invoke(false);
            }
        }
    }

    private void ShowError(string message)
    {
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RetryInternetCheck);

        lastErrorMessage = message;
        errorPanel.SetActive(true);
        retryButton.gameObject.SetActive(true);
        if (errorText != null) errorText.text = message;
    }

    public void RetryInternetCheck()
    {
        Debug.Log("🔄 Újrapróbálkozás internetkapcsolattal...");
        errorPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
        CheckInternetConnection(retryCallback); // **Folytatja az eredeti folyamatot**
    }
}
