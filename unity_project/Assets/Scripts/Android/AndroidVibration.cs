using UnityEngine;

public class AndroidVibration : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void Vibrate(long milliseconds = 250)
    {
        if (SettingsManager.isVibrationOn)
        {
            Debug.Log("Rezgés elindítva.");
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");

                if (vibrator != null)
                {
                    AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION");
                    int sdkInt = version.GetStatic<int>("SDK_INT");

                    if (sdkInt >= 26)
                    {
                        AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
                        AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, 255); // 255 = default amplitude
                        vibrator.Call("vibrate", effect);
                    }
                    else
                    {
                        vibrator.Call("vibrate", milliseconds);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Vibration error: " + e.Message);
        }
#endif
            Debug.Log("Rezgés vége.");
        }
    }
}
