using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera mainCamera;
    public BoardManager boardManager;
    private GameObject selectedObject;
    private VehicleController vehicleController;
    private VehicleInfo vehicleInfo;

    private Vector3 offset;
    private Vector3 touchWorldPosition;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit) && (hit.collider.CompareTag("Vehicle") || hit.collider.CompareTag("MainVehicle")))
                {
                    selectedObject = hit.collider.gameObject;
                    vehicleInfo = selectedObject.GetComponent<VehicleInfo>();
                    vehicleController = selectedObject.GetComponent<VehicleController>();

                    vehicleInfo.isDraggingRN = true;
                    boardManager.isDraggingVehicle = true;

                    // Az érintési pozíció kiszámítása
                    touchWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.WorldToScreenPoint(selectedObject.transform.position).z));

                    // Offset kiszámítása
                    offset = selectedObject.transform.position - touchWorldPosition;

                    vehicleController.OnTouchBegan();
                }
            }

            else if (touch.phase == TouchPhase.Moved && selectedObject != null)
            {
                // Az érintési pozíció frissítése
                touchWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.WorldToScreenPoint(selectedObject.transform.position).z));
                vehicleController.OnTouchMoved(touchWorldPosition + offset);
            }

            else if (touch.phase == TouchPhase.Ended && selectedObject != null)
            {
                vehicleInfo.isDraggingRN = false;
                boardManager.isDraggingVehicle = false;
                vehicleController.OnTouchEnded();

                selectedObject = null;
                vehicleController = null;
                vehicleInfo = null;
            }
        }
    }
}
