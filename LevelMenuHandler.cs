using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelMenuHandler : MonoBehaviour
{
    public static LevelMenuHandler instance;

    public GameObject buttonsLocation;
    public GameObject levelButtonPrefab; // A szintgombokhoz szükséges prefab
    private int totalLevels = 0; // A pályák száma

    private void Awake()
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

    public void LoadLevel(int levelIndex)
    {
        MenuSceneManager.selectedLevelIndex = levelIndex;
        SceneManager.LoadScene("GameScene");
        Debug.Log($"🔹 Betöltés: {levelIndex}. pálya");
    }

    // 🔹 Külsőleg is meghívható metódus a pályák gombjainak létrehozására
    public void CreateLevelMenuButtons()
    {
        GetTotalLevels(levelCount =>
        {
            totalLevels = levelCount;
            GenerateLevelButtons();
        }, error =>
        {
            Debug.LogError("❌ Hiba a pályák számának lekérésekor: " + error);
        });
    }

    // 🔹 Gombok generálása a `totalLevels` alapján
    private void GenerateLevelButtons()
    {
        Debug.Log($"✅ CreateLevelMenuButtons hívva. Pályák száma: {totalLevels}");

        if (buttonsLocation == null)
        {
            Debug.LogError("❌ HIBA: A `buttonsLocation` nincs beállítva az Inspectorban!");
            return;
        }

        if (levelButtonPrefab == null)
        {
            Debug.LogError("❌ HIBA: A `levelButtonPrefab` nincs beállítva az Inspectorban!");
            return;
        }

        // Előző gombok törlése, ha vannak
        foreach (Transform child in buttonsLocation.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= totalLevels; i++) // Szintek számozása 1-től
        {
            GameObject button = Instantiate(levelButtonPrefab, buttonsLocation.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{i}";
            int levelIndex = i - 1;
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }

        Debug.Log($"✅ {totalLevels} pálya gombjai létrehozva!");
    }

    // 🔹 A szerverről lekéri az összes elérhető pálya számát
    public void GetTotalLevels(Action<int> onSuccess, Action<string> onError)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            int levelCount = 0;

            foreach (KeyValuePair<string, string> entry in result.Data)
            {
                if (entry.Key.StartsWith("Level_")) // Csak a "Level_" kezdetű kulcsokat számolja
                {
                    levelCount++;
                }
            }

            Debug.Log($"🔹 Összes elérhető pálya a szerveren: {levelCount}");
            onSuccess?.Invoke(levelCount);

        }, error =>
        {
            Debug.LogError("❌ Hiba a pályák számának lekérésekor: " + error.GenerateErrorReport());
            onError?.Invoke(error.GenerateErrorReport());
        });
    }
}
