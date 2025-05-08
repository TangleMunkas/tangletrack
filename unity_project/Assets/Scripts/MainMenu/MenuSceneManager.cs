using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    public static MenuSceneManager instance;
    public static int selectedLevelIndex = 0; // Alapértelmezett szint
    public static bool isMainMenuActive = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LevelMenuHandler.instance.CreateLevelMenuButtons();
    }
}
