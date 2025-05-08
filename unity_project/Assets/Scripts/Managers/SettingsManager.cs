using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;

    public Toggle toggleMusic;
    public Toggle toggleSFX;
    public Toggle toggleVibration;
    public Toggle toggleLevelStartingAnimation;
    public Toggle toggleLowPerformanceMode;
    public Toggle toggleFPS;

    public static bool isMusicOn;
    public static bool isVibrationOn;
    public static bool isLevelStartingAnimationOn;
    public static bool isLowPerformanceModeOn;
    public static bool isShowingFPS;

    private bool isTogglesInitialized = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Duplikált példány
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);


        // Zene állapot betöltése és alkalmazása
        isMusicOn = true;
        if (PlayerPrefs.HasKey("musicOn"))
        {
            isMusicOn = PlayerPrefs.GetInt("musicOn") == 0;
        }
        if (toggleMusic != null) toggleMusic.isOn = isMusicOn;

        // SFX állapot betöltése és alkalmazása
        bool sfxOn = true;
        if (PlayerPrefs.HasKey("sfxOn"))
        {
            sfxOn = PlayerPrefs.GetInt("sfxOn") == 0;
        }
        if (toggleSFX != null) toggleSFX.isOn = sfxOn;
        SetSFXState(sfxOn);

        // Rezgés állapot
        isVibrationOn = true;
        if (PlayerPrefs.HasKey("vibration"))
        {
            isVibrationOn = PlayerPrefs.GetInt("vibration") == 0;
            if (toggleVibration != null) toggleVibration.isOn = isVibrationOn;
        }

        // Szint kezdési animáció állapot
        isLevelStartingAnimationOn = true;
        if (PlayerPrefs.HasKey("levelStartingAnimation"))
        {
            isLevelStartingAnimationOn = PlayerPrefs.GetInt("levelStartingAnimation") == 0;
            if (toggleLevelStartingAnimation != null) toggleLevelStartingAnimation.isOn = isLevelStartingAnimationOn;
        }

        // Alacsony teljesítmény mód
        isLowPerformanceModeOn = false;
        QualitySettings.vSyncCount = 0;
        int targetFPS = 120;
        if (PlayerPrefs.HasKey("lowPerformanceMode"))
        {
            isLowPerformanceModeOn = PlayerPrefs.GetInt("lowPerformanceMode") == 0;
            targetFPS = isLowPerformanceModeOn ? 30 : 120;
            if (toggleLowPerformanceMode != null) toggleLowPerformanceMode.isOn = isLowPerformanceModeOn;
        }
        Application.targetFrameRate = targetFPS;

        // FPS kijelzõ állapot
        isShowingFPS = false;
        if (PlayerPrefs.HasKey("showFPS"))
        {
            isShowingFPS = PlayerPrefs.GetInt("showFPS") == 0;
            if (toggleFPS != null) toggleFPS.isOn = isShowingFPS;
        }

        isTogglesInitialized = true;
    }

    public void ChangeMusicState()
    {
        if (isTogglesInitialized)
        {
            isMusicOn = !isMusicOn;
            PlayerPrefs.SetInt("musicOn", isMusicOn ? 0 : 1);
            MusicManager.valueChanged = true;

            SFXManager.instance.PlayClick();
        }
    }

    public void ChangeSFXState()
    {
        if (isTogglesInitialized)
        {
            AudioSource audioSource = SFXManager.instance.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                bool newState = !(audioSource.volume > 0f);
                audioSource.volume = newState ? 1f : 0f;
                PlayerPrefs.SetInt("sfxOn", newState ? 0 : 1);

            }

            SFXManager.instance.PlayClick();
        }
    }

    public void ChangeVibrationState()
    {
        if (isTogglesInitialized)
        {
            isVibrationOn = !isVibrationOn;
            PlayerPrefs.SetInt("vibration", isVibrationOn ? 0 : 1);

            SFXManager.instance.PlayClick();
        }
    }

    public void ChangeLevelStartingAnimationState()
    {
        if (isTogglesInitialized)
        {
            isLevelStartingAnimationOn = !isLevelStartingAnimationOn;
            PlayerPrefs.SetInt("levelStartingAnimation", isLevelStartingAnimationOn ? 0 : 1);

            SFXManager.instance.PlayClick();
        }
    }

    public void ChangePerformanceState()
    {
        if (isTogglesInitialized)
        {
            isLowPerformanceModeOn = !isLowPerformanceModeOn;
            PlayerPrefs.SetInt("lowPerformanceMode", isLowPerformanceModeOn ? 0 : 1);
            Application.targetFrameRate = isLowPerformanceModeOn ? 30 : 120;
            BackgroundVehicleManager.valueChanged = true;

            SFXManager.instance.PlayClick();
        }
    }

    public void ChangeShowFPSState()
    {
        if (isTogglesInitialized)
        {
            isShowingFPS = !isShowingFPS;
            PlayerPrefs.SetInt("showFPS", isShowingFPS ? 0 : 1);

            SFXManager.instance.PlayClick();
        }
    }

    private void SetSFXState(bool isOn)
    {
        AudioSource sfxSource = SFXManager.instance.GetComponent<AudioSource>();
        if (sfxSource != null)
        {
            sfxSource.volume = isOn ? 1f : 0f;
        }
    }
}