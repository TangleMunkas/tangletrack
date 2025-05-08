using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoBehaviour
{
    //public GameManager GameManager;
    public InGameUIManager inGameUIManager;
    public GameObject vehicleContainer;
    public bool isFinishCheckActive = false;
    private Transform mainVehicleTransform;

    public void FindMainVehicle()
    {
        //Debug.Log("FindMainVehicle() meghívva.");
        foreach (Transform child in vehicleContainer.transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("MainVehicle"))
            {
                //Debug.Log("MainVehicle taggel ellátott objektum megtalálva.");
                mainVehicleTransform = child.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (isFinishCheckActive && mainVehicleTransform != null)
        {
            if (mainVehicleTransform.position.x >= 6.5f)
            {
                //collision.gameObject.GetComponent<IndicateAlign>().DestroyIndicateAlign();
                inGameUIManager.GameFinished();
                AndroidVibration.Vibrate(200);
                //GameManager.GameFinished();

                isFinishCheckActive = false;
            }
        }
    }
}