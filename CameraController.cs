using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Az objektum, ami körül forog a kamera
    public float distance = 10.0f; // Távolság az objektumtól
    public float rotationSpeed = 0.05f; // Forgási sebesség
    public float zoomSpeed = 0.02f; // Zoom sebesség
    public float minZoom = 8.0f; // Minimális távolság
    public float maxZoom = 18.0f; // Maximális távolság
    public float yMinLimit = 10f; // Minimum dõlésszög (függõleges)
    public float yMaxLimit = 89f;  // Maximum dõlésszög (függõleges)

    private float currentX = 0.0f; // Az aktuális vízszintes forgás
    private float currentY = 70.0f; // Az aktuális függõleges forgás
    private float initialTouchDistance; // Kezdeti érintési távolság a zoomhoz

    public BoardManager boardManager;


    void Update()
    {
        // Forgatás egyujjas érintéssel
        if (Input.touchCount == 1 && !boardManager.isDraggingVehicle)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * rotationSpeed;
                currentY -= touch.deltaPosition.y * rotationSpeed;

                // Korlátozzuk a függõleges forgást
                currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
            }
        }

        // Zoom kétujjas érintéssel
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialTouchDistance = currentTouchDistance;
            }

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float distanceDelta = initialTouchDistance - currentTouchDistance;
                distance = Mathf.Clamp(distance + distanceDelta * zoomSpeed, minZoom, maxZoom);
                initialTouchDistance = currentTouchDistance;
            }
        }
    }

    void LateUpdate()
    {
        // Forgatás kiszámítása
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Kamera pozíciójának frissítése
        Vector3 direction = new Vector3(0, 0, -distance);
        transform.position = target.position + rotation * direction;

        // Kamera mindig a célpontra néz
        transform.LookAt(target);
    }
}
