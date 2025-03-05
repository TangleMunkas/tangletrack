using UnityEngine;
using System.Collections;

public class TransformChanger : MonoBehaviour
{
    public Vector3 targetRotationEuler; // Ahová nézzen
    public float duration = 0.5f; // Idõtartam másodpercben

    void Start()
    {
        TransformToMainMenu();
    }

    public void TransformToMainMenu()
    {
        targetRotationEuler = new Vector3(0, -60f, 0f);
        StartCoroutine(RotateOverTime());
    }

    public void TransformToLevelMenu()
    {
        targetRotationEuler = new Vector3(-45f, -60f, 0f);
        StartCoroutine(RotateOverTime());
    }

    private IEnumerator RotateOverTime()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Várjuk meg a következõ frame-et
        }

        transform.rotation = targetRotation; // Biztosítjuk, hogy pontosan a célértékre álljon
    }
}
