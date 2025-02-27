using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class LevelDataLoader : MonoBehaviour
{
    public static LevelDataLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GetLevelData(string levelKey)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(levelKey))
            {
                string jsonData = result.Data[levelKey];
                Debug.Log($"✅ Sikeresen lekértük a(z) {levelKey} adatait: " + jsonData);
            }
            else
            {
                Debug.LogError("❌ Nem található adat ezzel a kulccsal: " + levelKey);
            }
        }, error =>
        {
            Debug.LogError("❌ Hiba a TitleData lekérésénél: " + error.GenerateErrorReport());
        });
    }
}

// A szerveren tárolt adatok struktúrája
[Serializable]
public class LevelData
{
    public List<VehicleData> vehicles;
}