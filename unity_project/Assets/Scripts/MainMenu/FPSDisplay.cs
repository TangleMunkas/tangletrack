using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public static FPSDisplay instance;

    public TextMeshProUGUI fpsText;
    private float deltaTime;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Már van másik, ezt töröljük
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SettingsManager.isShowingFPS)
        {
            fpsText.gameObject.SetActive(true);
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }
        else
        {
            fpsText.gameObject.SetActive(false);
        }
    }
}
