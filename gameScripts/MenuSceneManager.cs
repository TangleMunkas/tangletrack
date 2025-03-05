using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    public GameObject persistentMenuCanvas;

    public static int selectedLevelIndex = 0; // Alapértelmezett szint

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Megõrzi az értékeket a jelenetek között
        DontDestroyOnLoad(persistentMenuCanvas);
    }

    private void Start()
    {
        LevelMenuHandler.instance.CreateLevelMenuButtons();
    }
}
