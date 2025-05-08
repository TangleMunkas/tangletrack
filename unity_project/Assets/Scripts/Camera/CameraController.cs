using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isUnlocked = true;
    public Transform target; // Az objektum, ami k�r�l forog a kamera
    public float distance = 10.0f; // T�vols�g az objektumt�l
    public float rotationSpeed = 0.05f; // Forg�si sebess�g
    public float zoomSpeed = 0.02f; // Zoom sebess�g
    public float minZoom = 8.0f; // Minim�lis t�vols�g
    public float maxZoom = 18.0f; // Maxim�lis t�vols�g
    public float yMinLimit = 10f; // Minimum d�l�ssz�g (f�gg�leges)
    public float yMaxLimit = 89f;  // Maximum d�l�ssz�g (f�gg�leges)

    private float currentX = 0.0f; // Az aktu�lis v�zszintes forg�s
    private float currentY = 70.0f; // Az aktu�lis f�gg�leges forg�s
    private float initialTouchDistance; // Kezdeti �rint�si t�vols�g a zoomhoz

    public InGameUIManager inGameUIManager;
    public BoardManager boardManager;


    void Update()
    {
        if (isUnlocked && !inGameUIManager.isGameFinished && !inGameUIManager.pausePanel.activeSelf && !CameraAnimations.isAnimationRunning)
        {
            // Forgat�s egyujjas �rint�ssel
            if (Input.touchCount == 1 && !boardManager.isDraggingVehicle)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    currentX += touch.deltaPosition.x * rotationSpeed;
                    currentY -= touch.deltaPosition.y * rotationSpeed;

                    // Korl�tozzuk a f�gg�leges forg�st
                    currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
                }
            }

            // Zoom k�tujjas �rint�ssel
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
    }

    void LateUpdate()
    {
        if (!CameraAnimations.isAnimationRunning)
        {
            // Forgat�s kisz�m�t�sa
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            // Kamera poz�ci�j�nak friss�t�se
            Vector3 direction = new Vector3(0, 0, -distance);
            transform.position = target.position + rotation * direction;

            // Kamera mindig a c�lpontra n�z
            transform.LookAt(target);
        }
    }
}
