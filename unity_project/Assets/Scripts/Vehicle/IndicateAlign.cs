using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicateAlign : MonoBehaviour
{
    private GameObject indicatorContainer;

    private VehicleInfo vehicleInfo;
    private VehicleController vehicleController;

    public GameObject indicatorPrefab;
    private GameObject indicatorObject;

    void Awake()
    {
        vehicleInfo = GetComponent<VehicleInfo>();
        vehicleController = GetComponent<VehicleController>();
    }

    private void Start()
    {
        indicatorContainer = GameObject.Find("IndicatorContainer");
    }

    public void SpawnIndicateAligns()
    {
        foreach (Transform child in indicatorContainer.transform)
        {
            Destroy(child.gameObject);
        }

        Vector3 alignedPosition = vehicleController.CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, new Vector3(transform.position.x, 0.501f, transform.position.z));
        Quaternion rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        indicatorObject = Instantiate(indicatorPrefab, alignedPosition, rotation, indicatorContainer.transform);
        indicatorObject.gameObject.tag = "AlignIndicator";
        indicatorObject.transform.localScale = new Vector3(Mathf.Round(transform.localScale.x), Mathf.Round(transform.localScale.z), 1); // Új méret beállítása ( indicatorObject.Y = gameObject.Z ; indicatorObject.X = gameObject.X)
    }

    public void MoveIndicateAligns()
    {
        indicatorObject.transform.position = vehicleController.CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, new Vector3(transform.position.x, 0.501f, transform.position.z));
    }

    public void DestroyIndicateAlign()
    {
        Destroy(indicatorObject);
    }
}
