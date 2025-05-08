using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject toggleLowPerformanceMode;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Duplikált példány
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ShowSettingsMenu()
    {
        SFXManager.instance.PlayClick();

        instance.settingsMenuPanel.SetActive(true);
        instance.toggleLowPerformanceMode.SetActive(MenuSceneManager.isMainMenuActive);
    }

    public static void HideSettingsMenu()
    {
        SFXManager.instance.PlayClick();

        instance.settingsMenuPanel.SetActive(false);
    }
}
