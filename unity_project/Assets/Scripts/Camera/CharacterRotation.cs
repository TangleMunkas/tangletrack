using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    public UI_Manager ui_manager;
    public float rotationSpeed = 0.2f;
    public float friction = 0.95f;
    public float returnDelay = 2f;
    public float returnSpeed = 1f;

    private Vector2 lastTouchPosition;
    private bool isTouching = false;
    private bool isValidTouch = false;
    private float currentRotationSpeed = 0f;

    private Quaternion startRotation;
    private float timeSinceTouch = 0f;
    private float returnProgress = 0f; // az ease-in interpolációhoz

    private bool isRotationBlocked = false;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (isRotationBlocked || !ui_manager.mainMenuPanel.activeSelf) return;

        if (!ui_manager.mainMenuPanel.activeSelf) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isTouching = true;

                Ray ray = uiCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    isValidTouch = hit.transform.CompareTag("MenuCharacter");
                }
                else
                {
                    isValidTouch = false;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isTouching && isValidTouch)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                currentRotationSpeed = -delta.x * rotationSpeed;
                transform.Rotate(Vector3.up, currentRotationSpeed, Space.World);
                lastTouchPosition = touch.position;
                timeSinceTouch = 0f;
                returnProgress = 0f;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
                isValidTouch = false;
                timeSinceTouch = 0f;
            }
        }

        if (!isTouching)
        {
            timeSinceTouch += Time.deltaTime;

            if (timeSinceTouch < returnDelay)
            {
                // Még nem kell visszafordulni, csak lendület lassul
                if (Mathf.Abs(currentRotationSpeed) > 0.01f)
                {
                    currentRotationSpeed *= friction;
                    transform.Rotate(Vector3.up, currentRotationSpeed, Space.World);
                }
            }
            else
            {
                // Visszafordulás ease-in interpolációval
                returnProgress += Time.deltaTime * returnSpeed;
                float t = Mathf.Clamp01(returnProgress);
                float easeIn = t * t; // Ease-in csak

                transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, easeIn);
            }
        }
    }

    public void ForceRotationReset()
    {
        transform.rotation = startRotation;
        currentRotationSpeed = 0f;
        isRotationBlocked = true;
    }

    public void EnableRotation()
    {
        isRotationBlocked = false;
    }
}
