using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;

    public TextMeshProUGUI levelText_1;
    public TextMeshProUGUI levelText_2;
    public TextMeshProUGUI xpProgressText;
    public TextMeshProUGUI titleText;
    public Slider sliderXP;

    private int totalXP = 0; // A szerver által tárolt XP
    private int currentLevel = 1;
    private int xpForNextLevel = 50;
    private int xpProgress = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetUserXp();
    }

    public void AddXP(int amount)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "addXPServer",
            FunctionParameter = new Dictionary<string, object> { { "amount", amount } },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, success =>
        {
            Debug.Log($"✅ {amount} XP sikeresen hozzáadva szerveroldalon!");
            GetUserXp(); // Frissítjük az XP-t a szerverről
        }, error =>
        {
            Debug.LogError("❌ Hiba az XP szerveroldali növelésénél: " + error.GenerateErrorReport());
        });
    }

    public void GetUserXp()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
            {
                //Debug.Log($"[DEBUG] Statok száma: {result.Statistics?.Count}");
                var xpStat = result.Statistics?.Find(stat => stat.StatisticName == "XP");

                if (xpStat != null)
                {
                    totalXP = xpStat.Value;

                    Debug.Log($"🔹 Játékos XP-je a szerveren: {totalXP}");

                    GetUserLevel(totalXP);
                    XpNeededToNextLevel();
                    UpdateXpUI();
                }
                else
                {
                    Debug.LogWarning("⚠️ XP statisztika nem található! Létrehozás 0 értékkel...");

                    var updateRequest = new UpdatePlayerStatisticsRequest
                    {
                        Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "XP", Value = 0 }
                    }
                    };

                    PlayFabClientAPI.UpdatePlayerStatistics(updateRequest, updateResult =>
                    {
                        Debug.Log("✅ XP statisztika létrehozva 0 értékkel.");
                        GetUserXp(); // újrahívás, most már biztosan létezni fog
                    },
                    updateError =>
                    {
                        Debug.LogError("❌ Nem sikerült létrehozni az XP statisztikát: " + updateError.GenerateErrorReport());
                    });
                }
            },
            error =>
            {
                Debug.LogError("❌ Hiba az XP lekérésénél: " + error.GenerateErrorReport());
            });
        }
        else
        {
            Debug.LogError("❌ Nem vagy bejelentkezve, nem lehet lekérdezni az XP-det!");
        }
    }


    public void GetUserLevel(int xp)
    {
        int level = 1;
        int xpNeeded = 50;  // Az első szinthez szükséges XP
        int xpAccumulated = 0;

        // 1-40. szintek: XP = (szint * 50)
        while (level < 40)
        {
            xpAccumulated += xpNeeded;
            if (xp < xpAccumulated) break;
            level++;
            xpNeeded = (level + 1) * 50;
        }

        // 41-60. szintek: XP = (szint * 100)
        while (level >= 40 && level < 60)
        {
            xpNeeded = (level + 1) * 100;
            xpAccumulated += xpNeeded;
            if (xp < xpAccumulated) break;
            level++;
        }

        // 61-100. szintek: XP = (szint * 200)
        while (level >= 60 && level < 100)
        {
            xpNeeded = (level + 1) * 200;
            xpAccumulated += xpNeeded;
            if (xp < xpAccumulated) break;
            level++;
        }

        currentLevel = level;
        xpProgress = xp - (xpAccumulated - xpNeeded);
        xpForNextLevel = xpNeeded;
    }

    public void XpNeededToNextLevel()
    {
        xpProgressText.text = $"{xpProgress}/{xpForNextLevel}";
    }

    void UpdateXpUI()
    {
        if (levelText_1 != null) levelText_1.text = $"{currentLevel}";
        if (levelText_2 != null) levelText_2.text = $"{currentLevel}";
        if (xpProgressText != null) xpProgressText.text = $"{xpProgress}/{xpForNextLevel}";
        if (sliderXP != null) sliderXP.value = (float)xpProgress / xpForNextLevel;

        if (titleText != null)
        {
            titleText.text = GetTitleForLevel(currentLevel);
        }
    }

    private string GetTitleForLevel(int level)
    {
        if (level > 99) return "Tangle God";
        if (level > 89) return "Grid Genius";
        if (level > 79) return "Labyrinth Master";
        if (level > 69) return "Mind Mapper";
        if (level > 59) return "Route Strategist";
        if (level > 49) return "Traffic Engineer";
        if (level > 39) return "Puzzle Architect";
        if (level > 29) return "Circuit Navigator";
        if (level > 19) return "Maze Explorer";
        if (level > 9) return "Pathfinder";
        return "Tangler";
    }
}
