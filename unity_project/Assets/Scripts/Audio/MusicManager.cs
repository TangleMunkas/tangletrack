using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static bool valueChanged = true;

    private void Start()
    {
        valueChanged = true;
    }

    private void Update()
    {
        if (valueChanged)
        {
            if (SettingsManager.isMusicOn)
            {
                GetComponent<AudioSource>().volume = 0.5f;
            }
            else
            {
                GetComponent<AudioSource>().volume = 0f;
            }
            valueChanged = false;
        }
    }
}