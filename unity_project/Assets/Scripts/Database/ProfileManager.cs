using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ProfileManager : MonoBehaviour
{
    public TextMeshProUGUI textPlayerID;
    public TextMeshProUGUI textDisplayName;
    public Image flagImage;
    public Sprite defaultFlagSprite;

    public Button buttonMore;
    public GameObject panelMore;
    private bool isMorePanelActive = false;

    public GameObject panelNameChanger;
    public TMP_InputField inputFieldName;
    public Button buttonChangeName;
    public TextMeshProUGUI errorText;

    public GameObject toastText;

    public GameObject panelIconChanger;
    public Transform gridIconContent;
    public GameObject iconButtonPrefab;
    public Sprite[] iconSprites; // 19 db ikon sprite
    public Image profilImage_1;
    public Image profilImage_2;

    private string displayName = "Unknown";
    private string playerID = "";

    private int currentIconId = 1;
    private int selectedIconId = -1;

    void Start()
    {
        GetUserProfile();

        inputFieldName.onValueChanged.AddListener(delegate { ValidateNameInput(); });
        inputFieldName.onEndEdit.AddListener(delegate { ValidateNameInput(); });
    }

    private void GetUserProfile()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
            {
                displayName = result.AccountInfo?.TitleInfo?.DisplayName ?? "Guest_Unknown";
                playerID = result.AccountInfo?.TitleInfo?.TitlePlayerAccount.Id ?? "";

                textPlayerID.text = playerID;
                textDisplayName.text = displayName;
            },
            error => Debug.LogError("❌ Hiba a DisplayName lekérése közben: " + error.GenerateErrorReport()));

            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
            {
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowLocations = true
                }
            },
            result =>
            {
                var locations = result.PlayerProfile?.Locations;
                if (locations != null && locations.Count > 0)
                {
                    string countryCode = locations[0].CountryCode?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(countryCode))
                        StartCoroutine(DownloadFlagImage(countryCode));
                    else
                        flagImage.sprite = defaultFlagSprite;
                }
                else
                {
                    flagImage.sprite = defaultFlagSprite;
                }
            },
            error => Debug.LogError("❌ Hiba a helyadatok lekérése közben: " + error.GenerateErrorReport()));

            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                if (result.Data != null && result.Data.ContainsKey("ProfileIconId"))
                {
                    int.TryParse(result.Data["ProfileIconId"].Value, out currentIconId);
                }
                else
                {
                    currentIconId = 1;
                }

                profilImage_1.sprite = iconSprites[currentIconId - 1];
                profilImage_2.sprite = iconSprites[currentIconId - 1];
            },
            error => Debug.LogError("❌ Hiba a profil ikon adat lekérése közben: " + error.GenerateErrorReport()));
        }
    }

    public void ButtonMorePressed()
    {
        SFXManager.instance.PlayClick();
        isMorePanelActive = !isMorePanelActive;
        panelMore.SetActive(isMorePanelActive);
    }

    public void ButtonOpenChangeNameMenuPressed()
    {
        SFXManager.instance.PlayClick();
        inputFieldName.text = displayName;
        panelNameChanger.SetActive(true);
        errorText.text = "";
        ValidateNameInput();
    }

    public void ButtonChangePressed()
    {
        SFXManager.instance.PlayClick();
        errorText.text = "";

        if (!IsNameInputValid(out string validationError))
        {
            Debug.LogWarning("⚠️ Hibás név megadás: " + validationError);
            errorText.text = validationError;
            return;
        }

        string newName = inputFieldName.text.Trim();

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        },
        result =>
        {
            Debug.Log($"✅ A név sikeresen frissítve: {newName}");
            displayName = newName;
            textDisplayName.text = displayName;
            panelNameChanger.SetActive(false);
            panelMore.SetActive(false);
        },
        error =>
        {
            Debug.LogError("❌ Hiba történt a név frissítése közben: " + error.GenerateErrorReport());
            errorText.text = "An unknown error occurred while updating your name.";
        });
    }

    public void ButtonCancelNameChangePressed()
    {
        SFXManager.instance.PlayClick();
        panelNameChanger.SetActive(false);
    }

    private void ValidateNameInput()
    {
        IsNameInputValid(out _);
    }

    private bool IsNameInputValid(out string errorMessage)
    {
        string newName = inputFieldName.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            errorMessage = "The name cannot be empty.";
            return false;
        }

        if (newName.Length < 3)
        {
            errorMessage = "The name is too short. Minimum 3 characters required.";
            return false;
        }

        if (newName.Length > 20)
        {
            errorMessage = "The name is too long. Maximum 20 characters allowed.";
            return false;
        }

        string pattern = @"^[A-Za-z0-9ÁÉÍÓÖŐÚÜŰáéíóöőúüű \-_]+$";
        if (!Regex.IsMatch(newName, pattern))
        {
            errorMessage = "The name can only contain letters, numbers, spaces, hyphens, and underscores.";
            return false;
        }

        if (newName == displayName)
        {
            errorMessage = "The new name must be different from the current name.";
            return false;
        }

        errorMessage = "";
        return true;
    }

    public void CopyPlayerIdToClipboard()
    {
        SFXManager.instance.PlayClick();
        if (!string.IsNullOrEmpty(playerID))
        {
            GUIUtility.systemCopyBuffer = playerID;
            Debug.Log("✅ A játékos azonosító a vágólapra másolva: " + playerID);
            ShowToast();
            panelMore.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ A játékos azonosító még nem érhető el.");
        }
    }

    private void ShowToast()
    {
        if (toastText != null)
        {
            toastText.SetActive(true);
            CancelInvoke("HideToast");
            Invoke("HideToast", 2f);
        }
    }

    private void HideToast()
    {
        if (toastText != null)
        {
            toastText.SetActive(false);
        }
    }

    private IEnumerator DownloadFlagImage(string countryCode)
    {
        string url = $"https://flagcdn.com/w80/{countryCode}.png";

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            flagImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            Debug.LogWarning("⚠️ Nem sikerült letölteni a zászlót, az alapértelmezett lesz.");
            flagImage.sprite = defaultFlagSprite;
        }
    }

    // --------------- Ikonválasztó rendszer ---------------

    private void GenerateIcons()
    {
        foreach (Transform child in gridIconContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < iconSprites.Length; i++)
        {
            int index = i + 1; // 1-től számozunk
            GameObject iconButton = Instantiate(iconButtonPrefab, gridIconContent);
            Image iconImage = iconButton.GetComponent<Image>(); // saját Image komponens
            iconImage.sprite = iconSprites[i];

            Button button = iconButton.GetComponent<Button>();
            button.onClick.AddListener(() => SelectIcon(index, iconButton));

            if (index == currentIconId)
            {
                iconButton.transform.Find("BorderSelected").gameObject.SetActive(true);
                selectedIconId = index;
            }
            else
            {
                iconButton.transform.Find("BorderSelected").gameObject.SetActive(false);
            }
        }
    }

    private void SelectIcon(int index, GameObject buttonObj)
    {
        selectedIconId = index;

        foreach (Transform child in gridIconContent)
        {
            child.Find("BorderSelected").gameObject.SetActive(false);
        }

        buttonObj.transform.Find("BorderSelected").gameObject.SetActive(true);
    }

    public void ButtonOpenIconChangerPressed()
    {
        SFXManager.instance.PlayClick();
        panelIconChanger.SetActive(true);
        GenerateIcons();
    }

    public void ButtonSaveIconPressed()
    {
        SFXManager.instance.PlayClick();
        if (selectedIconId <= 0)
        {
            Debug.LogWarning("⚠️ Nincs ikon kiválasztva.");
            return;
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "ProfileIconId", selectedIconId.ToString() }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,
        result =>
        {
            Debug.Log("✅ Profil ikon sikeresen elmentve: " + selectedIconId);
            currentIconId = selectedIconId;

            profilImage_1.sprite = iconSprites[currentIconId - 1];
            profilImage_2.sprite = iconSprites[currentIconId - 1];

            panelIconChanger.SetActive(false);
        },
        error =>
        {
            Debug.LogError("❌ Hiba az ikon mentésekor: " + error.GenerateErrorReport());
        });
    }

    public void ButtonCancelIconChangePressed()
    {
        SFXManager.instance.PlayClick();
        panelIconChanger.SetActive(false);
    }
}