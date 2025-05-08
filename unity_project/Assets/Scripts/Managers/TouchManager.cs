using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera mainCamera;
    public BoardManager boardManager;
    public InGameUIManager inGameUIManager;
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
            if (!inGameUIManager.isGameFinished && !inGameUIManager.pausePanel.activeSelf)
            {
                if (inGameUIManager.magicUsingSate == 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = mainCamera.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            GameObject hitObject = hit.collider.gameObject;

                            // Ha a collider gyermek objektumé, keressük meg a szülõt
                            if (hitObject.CompareTag("Vehicle") || hitObject.CompareTag("MainVehicle"))
                            {
                                selectedObject = hitObject;
                            }
                            else if (hitObject.transform.parent != null &&
                                     (hitObject.transform.parent.CompareTag("Vehicle") || hitObject.transform.parent.CompareTag("MainVehicle")))
                            {
                                selectedObject = hitObject.transform.parent.gameObject;
                            }
                            else
                            {
                                selectedObject = null;
                            }

                            if (selectedObject != null)
                            {
                                vehicleInfo = selectedObject.GetComponent<VehicleInfo>();
                                vehicleController = selectedObject.GetComponent<VehicleController>();

                                vehicleInfo.isDraggingRN = true;
                                boardManager.isDraggingVehicle = true;

                                touchWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(
                                    touch.position.x, touch.position.y,
                                    mainCamera.WorldToScreenPoint(selectedObject.transform.position).z));

                                offset = selectedObject.transform.position - touchWorldPosition;

                                vehicleController.OnTouchBegan();
                            }
                        }
                    }

                    else if (touch.phase == TouchPhase.Moved && selectedObject != null)
                    {
                        // Az érintési pozíció frissítése
                        touchWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.WorldToScreenPoint(selectedObject.transform.position).z));
                        if (vehicleController != null)
                        {
                            vehicleController.OnTouchMoved(touchWorldPosition + offset);
                        }
                    }

                    else if (touch.phase == TouchPhase.Ended && selectedObject != null)
                    {
                        if (vehicleInfo != null)
                        {
                            vehicleInfo.isDraggingRN = false;
                        }
                        boardManager.isDraggingVehicle = false;
                        if (vehicleController != null)
                        {
                            vehicleController.OnTouchEnded();
                        }

                        selectedObject = null;
                        vehicleController = null;
                        vehicleInfo = null;
                    }
                }
                else if (inGameUIManager.magicUsingSate == 2 || inGameUIManager.magicUsingSate == 3)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = mainCamera.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            GameObject hitObject = hit.collider.gameObject;

                            // Ha a collider gyermek objektumé, keressük meg a szülõt
                            if (hitObject.CompareTag("Vehicle"))
                            {
                                selectedObject = hitObject;
                            }
                            else if (hitObject.transform.parent != null && hitObject.transform.parent.CompareTag("Vehicle"))
                            {
                                selectedObject = hitObject.transform.parent.gameObject;
                            }
                            else
                            {
                                selectedObject = null;
                            }

                            inGameUIManager.VehicleSelectedToUseMagic(selectedObject);
                        }
                    }
                }
            }
        }
    }
}
