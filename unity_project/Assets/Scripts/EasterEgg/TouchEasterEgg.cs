using UnityEngine;
using System.Collections;

public class TouchEasterEgg : MonoBehaviour
{
    [SerializeField] private int requiredTaps = 5;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float decayRate = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private string animationTriggerName = "PlayClap";
    [SerializeField] private float cooldownDuration = 3f;

    private int currentTapCount = 0;
    private float timeSinceLastTap = 0f;
    private bool isCooldown = false;

    private CharacterRotation characterRotation;

    private void Start()
    {
        characterRotation = GetComponent<CharacterRotation>();
    }

    private void Update()
    {
        if (isCooldown) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    currentTapCount++;
                    timeSinceLastTap = 0f;

                    if (currentTapCount >= requiredTaps)
                    {
                        ActivateEasterEgg();
                    }
                }
            }
        }

        if (currentTapCount > 0)
        {
            timeSinceLastTap += Time.deltaTime;

            if (timeSinceLastTap >= 1f)
            {
                currentTapCount = Mathf.Max(0, currentTapCount - Mathf.FloorToInt(timeSinceLastTap / decayRate));
                timeSinceLastTap = 0f;
            }
        }
    }

    private void ActivateEasterEgg()
    {
        Debug.Log("🎉 Easter egg aktiválva!");
        currentTapCount = 0;
        isCooldown = true;

        if (characterRotation != null)
        {
            characterRotation.ForceRotationReset(); // visszafordítjuk és letiltjuk ideiglenesen a forgatást
        }

        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
            SFXManager.instance.PlayHorn();
        }

        StartCoroutine(ResetCooldownAfterDelay());
    }

    private IEnumerator ResetCooldownAfterDelay()
    {
        yield return new WaitForSeconds(cooldownDuration);

        if (characterRotation != null)
        {
            characterRotation.EnableRotation();
        }

        isCooldown = false;
    }
}
