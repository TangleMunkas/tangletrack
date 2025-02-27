using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;

    public Text xpText;
    public TextMeshProUGUI levelText;
    public Text xpProgressText;

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
                foreach (var stat in result.Statistics)
                {
                    if (stat.StatisticName == "XP")
                    {
                        totalXP = stat.Value;
                        xpText.text = stat.Value.ToString();
                        Debug.Log($"🔹 Játékos XP-je a szerveren: {totalXP}");

                        // Frissítjük a szintet és az XP állapotot
                        GetUserLevel(totalXP);
                        XpNeededToNextLevel();
                        UpdateXpUI();
                    }
                }
            }, error =>
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
        if (levelText != null) levelText.text = $"{currentLevel}";
        if (xpProgressText != null) xpProgressText.text = $"{xpProgress}/{xpForNextLevel}";
    }
}
