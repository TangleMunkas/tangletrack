using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    public GameObject bottomMenuPanel;

    public GameObject storeMenuPanel;
    public GameObject cosmeticsMenuPanel;
    public GameObject mainMenuPanel;
    public GameObject friendsMenuPanel;
    public GameObject leaderboardMenuPanel;

    public GameObject levelMenuPanel;
    public GameObject profileMenuPanel;

    [Header("Scroll View Settings")]
    public ScrollRect scrollRect; // A Scroll Rect referencia
    public float scrollSpeed = 1f; // Görgetési sebesség

    // Elõre definiált célpozíciók a négy elemhez (1 = teteje, 0 = alja)
    private float position_MagicItems = 1f;
    private float position_Cosmetics = 0.75f;
    private float position_Openable = 0.5f;
    private float position_Coin = 0.35f;
    private float position_Bucks = 0f;

    void Start()
    {
        MenuSceneManager.isMainMenuActive = true;
        ChangeMenuToMain();
    }

    public void ChangeMenuToStore()
    {
        SFXManager.instance.PlayClick();
        storeMenuPanel.SetActive(true);

        cosmeticsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        friendsMenuPanel.SetActive(false);
        leaderboardMenuPanel.SetActive(false);
    }

    public void ChangeMenuToCosmetics()
    {
        SFXManager.instance.PlayClick();
        cosmeticsMenuPanel.SetActive(true);

        storeMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        friendsMenuPanel.SetActive(false);
        leaderboardMenuPanel.SetActive(false);
    }

    public void ChangeMenuToMain()
    {
        mainMenuPanel.SetActive(true);
        bottomMenuPanel.SetActive(true);

        storeMenuPanel.SetActive(false);
        cosmeticsMenuPanel.SetActive(false);
        friendsMenuPanel.SetActive(false);
        leaderboardMenuPanel.SetActive(false);

        levelMenuPanel.SetActive(false);
    }

    public void ChangeMenuToFriends()
    {
        SFXManager.instance.PlayClick();
        friendsMenuPanel.SetActive(true);

        storeMenuPanel.SetActive(false);
        cosmeticsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        leaderboardMenuPanel.SetActive(false);
    }

    public void ChangeMenuToLeaderBoard()
    {
        SFXManager.instance.PlayClick();
        leaderboardMenuPanel.SetActive(true);

        storeMenuPanel.SetActive(false);
        cosmeticsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        friendsMenuPanel.SetActive(false);
    }

    public void ChangeToLevelMenu()
    {
        SFXManager.instance.PlayClick();
        levelMenuPanel.SetActive(true);

        mainMenuPanel.SetActive(false);
        bottomMenuPanel.SetActive(false);
    }

    public void ChangeToSettingsMenu()
    {
        PersistentCanvas.ShowSettingsMenu();
    }

    public void ShowProfileMenu()
    {
        SFXManager.instance.PlayClick();
        profileMenuPanel.SetActive(true);
    }

    public void HideProfileMenu()
    {
        SFXManager.instance.PlayClick();
        profileMenuPanel.SetActive(false);
    }

    public void JumpToFastBuy(string itemName)
    {
        SFXManager.instance.PlayClick();
        storeMenuPanel.SetActive(true);

        mainMenuPanel.SetActive(false);

        float targetPosition = 1.0f; // Alapértelmezett érték

        switch (itemName)
        {
            case "MagicItems":
                targetPosition = position_MagicItems;
                break;
            case "Cosmetics":
                targetPosition = position_Cosmetics;
                break;
            case "Openable":
                targetPosition = position_Openable;
                break;
            case "Coin":
                targetPosition = position_Coin;
                break;
            case "Bucks":
                targetPosition = position_Bucks;
                break;
            default:
                Debug.LogWarning("Nincs ilyen menüpont: " + itemName);
                return;
        }

        StartCoroutine(SmoothScrollTo(targetPosition)); // Elindítja a görgetést
    }

    private IEnumerator SmoothScrollTo(float targetPosition)
    {
        float startPosition = scrollRect.verticalNormalizedPosition;
        float elapsedTime = 0f;
        float duration = 0.5f; // Fél másodperces átmenet

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * scrollSpeed;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = targetPosition; // Pontosan beállítjuk a végsõ értéket
    }

    public void BuyMagicReduce()  // Coin-ért cserébe Magic Reduce
    {
        int price = 200;

        if (CurrencyManager.instance == null)
        {
            Debug.LogError("❌ CurrencyManager példány nem található!");
            return;
        }

        if (CurrencyManager.instance.GetCoins() >= price)
        {
            SFXManager.instance.PlayBuying();
            CurrencyManager.instance.SubtractCoins(price);
            CurrencyManager.instance.AddMagicReduce(1);
            Debug.Log("✅ Magic Reduce vásárlása sikeres.");
        }
        else
        {
            Debug.LogWarning("⚠️ Nincs elég coin a vásárláshoz.");
        }
    }

    public void BuyMagicRemove() // Coin-ért cserébe Magic Remove
    {
        int price = 350;

        if (CurrencyManager.instance == null)
        {
            Debug.LogError("❌ CurrencyManager példány nem található!");
            return;
        }

        if (CurrencyManager.instance.GetCoins() >= price)
        {
            SFXManager.instance.PlayBuying();
            CurrencyManager.instance.SubtractCoins(price);
            CurrencyManager.instance.AddMagicRemove(1);
            Debug.Log("✅ Magic Remove vásárlása sikeres.");
        }
        else
        {
            Debug.LogWarning("⚠️ Nincs elég coin a vásárláshoz.");
        }
    }

    public void BuyCoin(int coinAmount)
    {
        int priceInBucks = coinAmount / 10;

        if (CurrencyManager.instance == null)
        {
            Debug.LogError("❌ CurrencyManager példány nem található!");
            return;
        }

        if (CurrencyManager.instance.GetBucks() >= priceInBucks)
        {
            SFXManager.instance.PlayBuying();
            CurrencyManager.instance.SubtractBucks(priceInBucks);
            CurrencyManager.instance.AddCoins(coinAmount);
            Debug.Log($"✅ Vásárlás sikeres: {coinAmount} coin {priceInBucks} bucks-ért.");
        }
        else
        {
            Debug.LogWarning("⚠️ Nincs elég bucks a vásárláshoz.");
        }
    }

    public void BuyBucks(int bucksAmount) // Egyelõre feltétel nélkül jár Bucks (a tesztelés idejére)
    {
        SFXManager.instance.PlayBuying();
        CurrencyManager.instance.AddBucks(bucksAmount);
        Debug.Log("✅ Bucks jóváírva (teszt).");
    }
}
