using UnityEngine;
using System.Collections;

public class CameraAnimations : MonoBehaviour
{
    public InGameUIManager inGameUIManager;
    public static bool isAnimationRunning = true;

    [Header("Step 1: Emelked�s �s fordul�s")]
    public Vector3 riseTargetPosition = new Vector3(-20.6f, 17f, -44.3f);
    public Vector3 riseTargetRotationEuler = new Vector3(45f, 0f, 0f);
    public float riseDuration = 1f;

    [Header("Step 2: Elmozdul�s a c�lhoz")]
    public Vector3 finalPosition = new Vector3(3f, 17f, 3f);
    public float moveDuration = 1f;

    [Header("Step 3: Kis lefel� n�z�s")]
    public Vector3 finalLookEuler = new Vector3(70f, 0f, 0f);
    public float lookDownDuration = 0.5f;

    private void Start()
    {
        if (SettingsManager.isLevelStartingAnimationOn)
        {
            StartCoroutine(RunGameStartAnimation());
        }
        else
        {
            isAnimationRunning = false;
            inGameUIManager.FirstStart();
        }
    }

    public IEnumerator RunGameStartAnimation()
    {
        isAnimationRunning = true;

        yield return new WaitForSeconds(2.5f);

        // --- Step 1: Emelked�s �s fix rot�ci� ---
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion riseTargetRot = Quaternion.Euler(riseTargetRotationEuler);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / riseDuration;
            float eased = EaseInOut(t);
            transform.position = Vector3.Lerp(startPos, riseTargetPosition, eased);
            transform.rotation = Quaternion.Slerp(startRot, riseTargetRot, eased);
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        // --- Step 2: Mozg�s a c�lpoz�ci�ra ---
        Vector3 moveStartPos = transform.position;
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float eased = EaseInOut(t);
            transform.position = Vector3.Lerp(moveStartPos, finalPosition, eased);
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        // --- Step 3: Lefel� n�z�s fix rot�ci�ra ---
        Quaternion lookTargetRot = Quaternion.Euler(finalLookEuler);
        Quaternion currentRot = transform.rotation;
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / lookDownDuration;
            float eased = EaseInOut(t);
            transform.rotation = Quaternion.Slerp(currentRot, lookTargetRot, eased);
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        isAnimationRunning = false;
        inGameUIManager.FirstStart();
    }

    // Egyszer� ease-in-out f�ggv�ny (smoothstep)
    private float EaseInOut(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * (3f - 2f * t); // smoothstep g�rbe
    }
}
