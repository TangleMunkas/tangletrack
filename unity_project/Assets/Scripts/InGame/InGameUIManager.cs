using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public VehicleRefreshManager vehicleRefreshManager;
    public LevelDataLoader levelDataLoader;
    public FinishSystem finishSystem;
    public BoardManager boardManager;
    public GameObject gameMusic;
    public GameObject femalePolice;

    public GameObject topPanel;
    public GameObject magicPanel;
    public GameObject pausePanel;
    public GameObject endScreenPanel;
    public GameObject magicReduce;
    public GameObject magicRemove;
    public GameObject arrowIcon;
    public TextMeshProUGUI magicText;
    public Button buttonMagic;
    public Button buttonRestart;
    public Button buttonUse;
    public Button buttonCancel;
    public Sprite spriteButtonBack;
    public Sprite spriteButtonCancel;
    public CameraController cameraController;
    public Button cameraLockButton;
    public Sprite spriteCameraUnlocked;
    public Sprite spriteCameraLocked;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI levelText;

    // --------- End Screen -----------
    public TextMeshProUGUI ratingText;
    public Image star1;
    public Image star2;
    public Image star3;
    public TextMeshProUGUI levelText_2;
    public TextMeshProUGUI timeText_2;
    public Image timeTextBG;
    public Sprite spriteTimeTextBG_gold;
    public Sprite spriteTimeTextBG_gray;
    public TextMeshProUGUI movesText_2;
    public Image movesTextBG;
    public Sprite spriteMovesTextBG_gold;
    public Sprite spriteMovesTextBG_gray;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI xpText;
    public Button buttonNextLevel;
    // --------------------------------

    public TextMeshProUGUI magicReduceText;
    public TextMeshProUGUI magicRemoveText;
    public int magicUsingSate = 0; // 0 --> mágikus menü elrejtve ; 1 --> mágikus tárgy kiválasztása folyamatban ; 2 --> a céljármû kiválasztása folyamatban ; 3 --> a céljármû ki van választva és a felhasználás gombra vár a program
    private int selectedMagic = 0;
    private GameObject selectedVehicleObject;
    private GameObject indicatorContainer;

    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    private int vehicleMoves = 0;

    public bool isGameFinished = false;

    public void FirstStart()
    {
        indicatorContainer = GameObject.Find("IndicatorContainer");
        isGameFinished = false;
        finishSystem.isFinishCheckActive = true;
        elapsedTime = 0f;
        isTimerRunning = true;
        vehicleMoves = 0;
        timerText.text = "00:00";
        levelText.text = $"Level {MenuSceneManager.selectedLevelIndex + 1}";
        ShowTopPanel();

        if (PlayerPrefs.HasKey("cameraUnlocked")) // Elmentett beállítások betöltése
        {
            bool isUnlocked = PlayerPrefs.GetInt("cameraUnlocked") == 1;
            cameraController.isUnlocked = isUnlocked;
            cameraLockButton.GetComponent<Image>().sprite = isUnlocked ? spriteCameraLocked : spriteCameraUnlocked;
        }

        Destroy(femalePolice);
        SFXManager.instance.PlayHorn();

        StartCoroutine(WaitBeforeFindingMainVehicle(1f));
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void UpdateVehicleMoves()
    {
        vehicleMoves ++;
        movesText.text = $"{vehicleMoves}";
    }

    public void ShowTopPanel()
    {
        topPanel.SetActive(true);
        magicPanel.SetActive(false);
        magicUsingSate = 0;
        selectedMagic = 0;
    }

    public void ShowMagicPanel()
    {
        if (buttonMagic.IsInteractable())
        {
            CheckMagicItems();

            topPanel.SetActive(false);
            magicPanel.SetActive(true);
            magicUsingSate = 1;
        }
    }

    public void ButtonCancel()
    {
        SFXManager.instance.PlayClick();
        CheckMagicItems();

        switch (magicUsingSate)
        {
            case 1: // Mágikus menü elrejtése
                ShowTopPanel();
                magicUsingSate = 0;
                break;

            case 2: // Vissza a mágikus tárgy kiválasztásához
                selectedMagic = 0;
                ShowArrow(false);
                buttonCancel.GetComponent<Image>().sprite = spriteButtonBack;
                magicText.text = "Select a Magic Item first!";
                magicUsingSate = 1;
                break;

            case 3: // Vissza a céljármû kiválasztásához
                DestroyAllIndicateAligns();
                buttonUse.interactable = false;
                selectedVehicleObject = null;
                
                switch (selectedMagic)
                {
                    case 1:
                        magicText.text = "Select a vehicle to REDUCE!";
                        break;

                    case 2:
                        magicText.text = "Select a vehicle to REMOVE!";
                        break;
                }

                magicUsingSate = 2;
                break;
        }
    }

    public void ButtonUse()
    {
        if (buttonUse.IsInteractable())
        {
            switch (selectedMagic)
            {
                case 1: // Magic Reduce
                    SFXManager.instance.PlayMagic();
                    CurrencyManager.instance.SubtractMagicReduce(1, CheckMagicItems);
                    vehicleRefreshManager.magicItemReduce(selectedVehicleObject);
                    break;

                case 2: // Magic Destroy
                    SFXManager.instance.PlayMagic();
                    CurrencyManager.instance.SubtractMagicRemove(1, CheckMagicItems);
                    Destroy(selectedVehicleObject);
                    break;
            }

            DestroyAllIndicateAligns();
            buttonUse.interactable = false;
            ShowArrow(false);
            magicText.text = "";

            CheckMagicItems();
            ShowTopPanel();
            buttonMagic.interactable = false;
            StartCoroutine(EnableMagicButtonAfterDelay(3f)); // 3 másodperc után újra engedélyezve
        }
    }

    public void ButtonPause()
    {
        SFXManager.instance.PlayClick();
        topPanel.SetActive(false);
        pausePanel.SetActive(true);
        isTimerRunning = false;

        if (SettingsManager.isMusicOn)
        {
            gameMusic.GetComponent<AudioSource>().volume = 0.2f;
        }
    }

    public void ButtonContinue()
    {
        SFXManager.instance.PlayClick();
        topPanel.SetActive(true);
        pausePanel.SetActive(false);
        isTimerRunning = true;

        if (SettingsManager.isMusicOn)
        {
            gameMusic.GetComponent<AudioSource>().volume = 0.5f;
        }
    }

    public void ButtonSettings()
    {
        PersistentCanvas.ShowSettingsMenu();
    }

    public void ButtonRestart()
    {
        SFXManager.instance.PlayClick();
        topPanel.SetActive(true);
        endScreenPanel.SetActive(false);
        buttonRestart.interactable = false;
        levelDataLoader.RemoveCurrentVehicles();
        levelDataLoader.LoadLevelData(MenuSceneManager.selectedLevelIndex + 1);
        elapsedTime = 0;
        vehicleMoves = 0;
        isGameFinished = false;
        finishSystem.isFinishCheckActive = true;
        boardManager.isDraggingVehicle = false;
        movesText.text = "0";
        isTimerRunning = true;
        DestroyAllIndicateAligns();
        StartCoroutine(WaitBeforeFindingMainVehicle(1f));
        StartCoroutine(EnableRestartButtonAfterDelay(3f)); // 3 másodperc után újra engedélyezve
    }

    private IEnumerator EnableRestartButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttonRestart.interactable = true;
    }

    private IEnumerator EnableMagicButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttonMagic.interactable = true;
    }

    private IEnumerator WaitForSecondsBeforeResetStats(float delay)
    {
        yield return new WaitForSeconds(delay);
        elapsedTime = 0f;
        vehicleMoves = 0;
        movesText.text = "0";
    }

    private IEnumerator WaitBeforeFindingMainVehicle(float delay)
    {
        yield return new WaitForSeconds(delay);
        finishSystem.FindMainVehicle();
    }

    private IEnumerator TemporarilyLowerMusicVolume(float targetVolume, float duration)
    {
        AudioSource musicSource = gameMusic.GetComponent<AudioSource>();
        float originalVolume = musicSource.volume;
        musicSource.volume = targetVolume;

        yield return new WaitForSeconds(duration);

        musicSource.volume = originalVolume;
    }

    public void ButtonNextLevel()
    {
        if (buttonNextLevel.interactable)
        {
            SFXManager.instance.PlayClick();
            levelDataLoader.RemoveCurrentVehicles();
            MenuSceneManager.selectedLevelIndex++;
            levelDataLoader.LoadLevelData(MenuSceneManager.selectedLevelIndex + 1);
            levelText.text = $"Level {MenuSceneManager.selectedLevelIndex + 1}";
            endScreenPanel.SetActive(false);
            topPanel.SetActive(true);
            isGameFinished = false;
            finishSystem.isFinishCheckActive = true;
            isTimerRunning = true;
            StartCoroutine(WaitForSecondsBeforeResetStats(0.25f));
            StartCoroutine(WaitBeforeFindingMainVehicle(1f));
        }
    }

    public void GameFinished()
    {
        if (!isGameFinished)
        {
            if (SettingsManager.isMusicOn)
            {
                StartCoroutine(TemporarilyLowerMusicVolume(0.2f, 3f));
            }
            SFXManager.instance.PlayLevelComplete();
            isTimerRunning = false;
            topPanel.SetActive(false);
            RateLevel();

            buttonNextLevel.interactable = false;
            LevelDataLoader.instance.CheckIfLevelExists(MenuSceneManager.selectedLevelIndex + 2, exists =>
            {
                buttonNextLevel.interactable = exists;
            });

            endScreenPanel.SetActive(true);
            isGameFinished = true;
        }
    }

    private void RateLevel()
    {
        levelText_2.text = $"Level {MenuSceneManager.selectedLevelIndex + 1} completed!";
        timeText_2.text = $"{Mathf.FloorToInt(elapsedTime / 60f):00}:{Mathf.FloorToInt(elapsedTime % 60f):00}";
        movesText_2.text = $"{vehicleMoves + 1}"; // +1 az utolsó mozdulat miatt amit nem érzékel a rendszer

        int starsGiven = 1;

        if (elapsedTime < 90f)
        {
            starsGiven++;
            timeTextBG.sprite = spriteTimeTextBG_gold;
        }
        if (vehicleMoves < 20)
        {
            starsGiven++;
            movesTextBG.sprite = spriteMovesTextBG_gold;
        }

        Color currentColor = star1.GetComponent<Image>().color;
        currentColor.a = 0.15f;
        switch (starsGiven)
        {

            case 1:
                ratingText.text = "Not bad!";
                star2.GetComponent<Image>().color = currentColor;
                star3.GetComponent<Image>().color = currentColor;
                goldText.text = "50";
                CurrencyManager.instance.AddCoins(50);
                xpText.text = "+ 100xp";
                XpManager.instance.AddXP(100);
                break;

            case 2:
                ratingText.text = "Good job!";
                star3.GetComponent<Image>().color = currentColor;
                goldText.text = "100";
                CurrencyManager.instance.AddCoins(100);
                xpText.text = "+ 200xp";
                XpManager.instance.AddXP(200);
                break;

            case 3:
                ratingText.text = "Excellent!";
                goldText.text = "200";
                CurrencyManager.instance.AddCoins(200);
                xpText.text = "+ 300xp";
                XpManager.instance.AddXP(300);
                break;
        }
    }

    public void ButtonCameraLock()
    {
        SFXManager.instance.PlayClick();

        if (cameraController.isUnlocked)
        {
            cameraController.isUnlocked = false;
            cameraLockButton.GetComponent<Image>().sprite = spriteCameraUnlocked;
            PlayerPrefs.SetInt("cameraUnlocked", 0);
        }
        else
        {
            cameraController.isUnlocked = true;
            cameraLockButton.GetComponent<Image>().sprite = spriteCameraLocked;
            PlayerPrefs.SetInt("cameraUnlocked", 1);
        }

        PlayerPrefs.Save();
    }

    public void ButtonQuit()
    {
        SFXManager.instance.PlayClick();
        SceneManager.LoadScene("MenuScene");
    }

    public void MagicReduceSelected()
    {
        SFXManager.instance.PlayClick();
        if (magicUsingSate == 1 || magicUsingSate == 2)
        {
            selectedMagic = 1;
            magicUsingSate = 2;
            arrowIcon.transform.position = new Vector2(magicReduce.transform.position.x, arrowIcon.transform.position.y);
            ShowArrow(true);
            buttonUse.interactable = false;
            buttonCancel.GetComponent<Image>().sprite = spriteButtonCancel;
            magicText.text = "Select a vehicle to REDUCE!";
        }
        else // Ha a magicUsingState == 3
        {
            selectedMagic = 1;
            arrowIcon.transform.position = new Vector2(magicReduce.transform.position.x, arrowIcon.transform.position.y);
        }
    }

    public void MagicRemoveSelected()
    {
        SFXManager.instance.PlayClick();
        if (magicUsingSate == 1 || magicUsingSate == 2)
        {
            selectedMagic = 2;
            magicUsingSate = 2;
            arrowIcon.transform.position = new Vector2(magicRemove.transform.position.x, arrowIcon.transform.position.y);
            ShowArrow(true);
            buttonUse.interactable = false;
            buttonCancel.GetComponent<Image>().sprite = spriteButtonCancel;
            magicText.text = "Select a vehicle to REMOVE!";
        }
        else // Ha a magicUsingState == 3
        {
            selectedMagic = 2;
            arrowIcon.transform.position = new Vector2(magicRemove.transform.position.x, arrowIcon.transform.position.y);
        }
    }

    public void ShowArrow(bool showArrow)
    {
        if (showArrow)
        {
            arrowIcon.SetActive(true);
        }
        else
        {
            arrowIcon.SetActive(false);
        }
    }

    public void DestroyAllIndicateAligns()
    {
        foreach (Transform child in indicatorContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void VehicleSelectedToUseMagic(GameObject selectedVehicle)
    {
        if (selectedVehicle != null)
        {
            if (magicUsingSate == 2)
            {
                magicUsingSate = 3;
                magicText.text = "";
                buttonUse.interactable = true;
                selectedVehicleObject = selectedVehicle;
                selectedVehicleObject.GetComponent<IndicateAlign>().SpawnIndicateAligns();
            }
            else
            {
                DestroyAllIndicateAligns();
                selectedVehicleObject = selectedVehicle;
                selectedVehicleObject.GetComponent<IndicateAlign>().SpawnIndicateAligns();
            }
        }
    }

    private void CheckMagicItems()
    {
        CurrencyManager.instance.GetUserCurrency();

        int magicReducesLeft = CurrencyManager.instance.GetMagicReduce();
        int magicRemovesLeft = CurrencyManager.instance.GetMagicRemove();

        if (magicReducesLeft < 1)
        {
            magicReduce.GetComponent<Button>().interactable = false;
            magicReduceText.text = "0";
        }
        else
        {
            magicReduce.GetComponent<Button>().interactable = true;
            magicReduceText.text = $"{magicReducesLeft}";
        }

        if (magicRemovesLeft < 1)
        {
            magicRemove.GetComponent<Button>().interactable = false;
            magicRemoveText.text = "0";
        }
        else
        {
            magicRemove.GetComponent<Button>().interactable = true;
            magicRemoveText.text = $"{magicRemovesLeft}";
        }
    }
}
