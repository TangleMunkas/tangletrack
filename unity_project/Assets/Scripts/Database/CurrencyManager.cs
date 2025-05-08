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

    private int currentCoins = 0;
    private int currentBucks = 0;
    private int currentMagicReduce = 0;
    private int currentMagicRemove = 0;

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

        currentMagicReduce = result.VirtualCurrency.ContainsKey("RD") ? result.VirtualCurrency["RD"] : 0;
        currentMagicRemove = result.VirtualCurrency.ContainsKey("RV") ? result.VirtualCurrency["RV"] : 0;

        //Debug.Log($"💰 Coins: {currentCoins}, 💵 Bucks: {currentBucks}");
        UpdateCurrencyUI();
    }

    void OnGetUserInventoryFailure(PlayFabError error)
    {
        Debug.LogError("❌ Failed to get currency: " + error.GenerateErrorReport());
    }

    public void AddCoins(int quantity)
    {
        //Debug.Log($"[DEBUG] AddCoins hívás történt {quantity} értékkel.");
        currentCoins += quantity;
        UpdateCurrencyUI();
        ModifyCurrencyServer("TC", quantity);
    }
    public int GetCoins()
    {
        return currentCoins;
    }

    public void AddBucks(int quantity)
    {
        currentBucks += quantity;
        UpdateCurrencyUI();
        ModifyCurrencyServer("TB", quantity);
    }


    public int GetBucks()
    {
        return currentBucks;
    }

    public void AddMagicReduce(int quantity)
    {
        currentMagicReduce += quantity;
        ModifyCurrencyServer("RD", quantity);
    }

    public int GetMagicReduce()
    {
        return currentMagicReduce;
    }

    public void AddMagicRemove(int quantity)
    {
        currentMagicRemove += quantity;
        ModifyCurrencyServer("RV", quantity);
    }

    public int GetMagicRemove()
    {
        return currentMagicRemove;
    }

    public void SubtractCoins(int quantity)
    {
        if (currentCoins >= quantity)
        {
            currentCoins -= quantity;
            UpdateCurrencyUI();
            ModifyCurrencyServer("TC", -quantity);
        }
    }

    public void SubtractBucks(int quantity)
    {
        if (currentBucks >= quantity)
        {
            currentBucks -= quantity;
            UpdateCurrencyUI();
            ModifyCurrencyServer("TB", -quantity);
        }
    }

    public void SubtractMagicReduce(int quantity, System.Action onComplete = null)
    {
        ModifyCurrencyServer("RD", -quantity, onComplete);
    }

    public void SubtractMagicRemove(int quantity, System.Action onComplete = null)
    {
        ModifyCurrencyServer("RV", -quantity, onComplete);
    }

    private void ModifyCurrencyServer(string currencyCode, int amount, System.Action onComplete = null)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "addCurrencyServer",
            FunctionParameter = new Dictionary<string, object>
        {
            { "currencyCode", currencyCode },
            { "amount", amount }
        },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            Debug.Log($"✅ {amount} {currencyCode} sikeresen módosítva szerveroldalon!");
            //GetUserCurrency(); kikommentelve a szerveroldali ellenőrzés miatt
            UpdateCurrencyUI();
            onComplete?.Invoke(); // <-- itt hívjuk meg
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
    }
}
