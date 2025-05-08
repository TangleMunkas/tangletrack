using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelDataLoader : MonoBehaviour
{
    public static LevelDataLoader instance;
    public VehicleDataManager vehicleDataManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadLevelData(MenuSceneManager.selectedLevelIndex + 1);
    }

    public void LoadLevelData(int levelNumberToLoad)
    {
        GetLevelData("Level_" + levelNumberToLoad.ToString(), vehicleDataManager.LoadData);
    }

    private void GetLevelData(string levelKey, Action<List<VehicleData>> onSuccess)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(levelKey))
            {
                string jsonData = result.Data[levelKey];

                Debug.Log($"📜 JSON amit be akarunk olvasni: {jsonData}");

                try
                {
                    // A JSON beolvasása a megfelelő osztályba
                    LevelData levelDataWrapper = JsonConvert.DeserializeObject<LevelData>(jsonData);

                    if (levelDataWrapper == null)
                    {
                        Debug.LogError("❌ A JSON parszolás sikertelen: LevelData null értéket adott vissza!");
                        onSuccess?.Invoke(new List<VehicleData>());
                        return;
                    }

                    if (levelDataWrapper.saveVehicleDataList == null)
                    {
                        Debug.LogError("❌ A JSON beolvasás után a járművek listája NULL!");
                        onSuccess?.Invoke(new List<VehicleData>());
                        return;
                    }

                    Debug.Log($"✅ Beolvasott járművek száma: {levelDataWrapper.saveVehicleDataList.Count}");
                    onSuccess?.Invoke(levelDataWrapper.saveVehicleDataList);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ JSON parszolási hiba: {ex.Message}");
                    onSuccess?.Invoke(new List<VehicleData>());
                }
            }
            else
            {
                Debug.LogError("❌ Nem található adat ezzel a kulccsal: " + levelKey);
                onSuccess?.Invoke(new List<VehicleData>());
            }
        }, error =>
        {
            Debug.LogError("❌ Hiba a TitleData lekérésénél: " + error.GenerateErrorReport());
            onSuccess?.Invoke(new List<VehicleData>());
        });
    }

    public void CheckIfLevelExists(int levelIndex, Action<bool> callback)
    {
        string levelKey = "Level_" + levelIndex.ToString();

        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            bool exists = result.Data != null && result.Data.ContainsKey(levelKey);
            callback?.Invoke(exists);
        },
        error =>
        {
            Debug.LogError("Hiba a TitleData lekérésénél: " + error.GenerateErrorReport());
            callback?.Invoke(false);
        });
    }

    [Obsolete]
    public void RemoveCurrentVehicles()
    {
        GameObject vehicleContainer = GameObject.Find("VehicleContainer");

        if (vehicleContainer == null)
        {
            Debug.LogError("VehicleContainer not found in the scene!");
            return;
        }

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("Vehicle") || obj.CompareTag("MainVehicle"))
            {
                Destroy(obj);
            }
        }
    }
}

// A szerveren tárolt adatok struktúrája
[Serializable]
public class LevelData
{
    public List<VehicleData> saveVehicleDataList;
}
