using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    public GameObject persistentMenuCanvas;

    public static int selectedLevelIndex = 0; // Alap�rtelmezett szint

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Meg�rzi az �rt�keket a jelenetek k�z�tt
        DontDestroyOnLoad(persistentMenuCanvas);
    }

    private void Start()
    {
        LevelMenuHandler.instance.CreateLevelMenuButtons();
    }
}
