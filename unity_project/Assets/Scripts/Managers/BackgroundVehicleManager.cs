using UnityEngine;

public class BackgroundVehicleManager : MonoBehaviour
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
            if (SettingsManager.isLowPerformanceModeOn)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            valueChanged = false;
        }
    }
}