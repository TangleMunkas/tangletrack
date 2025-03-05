using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public TextMeshProUGUI coinText_Main;
    public TextMeshProUGUI bucksText_Main;

    public TextMeshProUGUI coinText_Store;
    public TextMeshProUGUI bucksText_Store;
    public TextMeshProUGUI magicReduceText;
    public TextMeshProUGUI magicRemoveText;
    public TextMeshProUGUI magicHologramText;

    private int currentCoins = 0;
    private int currentBucks = 0;
    private int currentMagicReduce = 0;
    private int currentMagicRemove = 0;
    private int currentMagicHologram = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GetUserCurrency();
    }


    public void GetUserCurrency()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnGetUserInventoryFailure);
        }
        else
        {
            Debug.LogError("❌ Nem vagy bejelentkezve, nem lehet lekérdezni az egyenleget!");
        }
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        currentCoins = result.VirtualCurrency.ContainsKey("TC") ? result.VirtualCurrency["TC"] : 0;
        currentBucks = result.VirtualCurrency.ContainsKey("TB") ? result.VirtualCurrency["TB"] : 0;

        currentMagicReduce = result.VirtualCurrency.ContainsKey("RV") ? result.VirtualCurrency["RV"] : 0;
        currentMagicRemove = result.VirtualCurrency.ContainsKey("RD") ? result.VirtualCurrency["RD"] : 0;
        currentMagicHologram = result.VirtualCurrency.ContainsKey("HG") ? result.VirtualCurrency["HG"] : 0;

        //Debug.Log($"💰 Coins: {currentCoins}, 💵 Bucks: {currentBucks}");
        UpdateCurrencyUI();
    }

    void OnGetUserInventoryFailure(PlayFabError error)
    {
        Debug.LogError("❌ Failed to get currency: " + error.GenerateErrorReport());
    }

    public void AddCoins(int quantity)
    {
        ModifyCurrencyServer("TC", quantity);
    }

    public void AddBucks(int quantity)
    {
        ModifyCurrencyServer("TB", quantity);
    }

    public void AddMagicReduce(int quantity)
    {
        ModifyCurrencyServer("RD", quantity);
    }

    public void AddMagicRemove(int quantity)
    {
        ModifyCurrencyServer("RV", quantity);
    }

    public void AddMagicHologram(int quantity)
    {
        ModifyCurrencyServer("HG", quantity);
    }

    public void SubtractCoins(int quantity)
    {
        ModifyCurrencyServer("TC", -quantity);
    }

    public void SubtractBucks(int quantity)
    {
        ModifyCurrencyServer("TB", -quantity);
    }

    public void SubtractMagicReduce(int quantity)
    {
        ModifyCurrencyServer("RD", -quantity);
    }

    public void SubtractMagicRemove(int quantity)
    {
        ModifyCurrencyServer("RV", -quantity);
    }

    public void SubtractMagicHologram(int quantity)
    {
        ModifyCurrencyServer("HG", -quantity);
    }


    private void ModifyCurrencyServer(string currencyCode, int amount)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "addCurrencyServer", // CloudScript függvény neve
            FunctionParameter = new Dictionary<string, object>
            {
                { "currencyCode", currencyCode },
                { "amount", amount }
            },
            GeneratePlayStreamEvent = true // Naplózás az eseményekhez
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            Debug.Log($"✅ {amount} {currencyCode} sikeresen módosítva szerveroldalon!");
            GetUserCurrency(); // Frissítsük az egyenleget szerveroldalról
            UpdateCurrencyUI();
        }, error =>
        {
            Debug.LogError("❌ Hiba a szerveroldali pénznem módosításakor: " + error.GenerateErrorReport());
        });
    }


    private void UpdateCurrencyUI()
    {
        if (coinText_Main != null) coinText_Main.text = currentCoins.ToString();
        if (bucksText_Main != null) bucksText_Main.text = currentBucks.ToString();
        if (coinText_Store != null) coinText_Store.text = currentCoins.ToString();
        if (bucksText_Store != null) bucksText_Store.text = currentBucks.ToString();

        if (magicReduceText != null) magicReduceText.text = currentMagicReduce.ToString();
        if (magicRemoveText != null) magicRemoveText.text = currentMagicRemove.ToString();
        if (magicHologramText != null) magicHologramText.text = currentMagicHologram.ToString();
    }
}
