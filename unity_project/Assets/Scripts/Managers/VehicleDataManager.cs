using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDataManager : MonoBehaviour
{
    public static VehicleDataManager instance;
    [SerializeField] private GameObject vehiclePrefab;

    public void LoadData(List<VehicleData> levelData)
    {
        Debug.Log("| VehicleDataManager --> LoadData | sikeresen elindult.");
        Debug.Log(levelData.Count);
        GameObject vehicleContainer = GameObject.Find("VehicleContainer");

        if (vehicleContainer == null)
        {
            Debug.LogError("VehicleContainer not found in the scene!");
            return;
        }
        
        foreach (VehicleData vehicleData in levelData)
        {
            GameObject vehicle = Instantiate(vehiclePrefab, vehicleContainer.transform);

            if (Application.isPlaying)
            {
                vehicle.GetComponent<VehicleInfo>().SetVehicleData(vehicleData);
            }
            else
            {
                #if UNITY_EDITOR
                    vehicle.GetComponent<VehicleEditor>().SetVehicleData(vehicleData);
                #endif
            }
        }
    }

    [System.Obsolete]
    public List<VehicleData> CreateSavingData()
    {
        List<GameObject> vehicles = new List<GameObject>();

        GameObject vehicleContainer = GameObject.Find("VehicleContainer");

        if (vehicleContainer == null)
        {
            Debug.LogError("VehicleContainer not found in the scene!");
            return null;
        }

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if ((obj.CompareTag("Vehicle") || obj.CompareTag("MainVehicle")) &&
                obj.GetComponent<VehicleController>() != null &&
                obj.transform.parent == vehicleContainer.transform)
            {
                vehicles.Add(obj);
            }
        }

        List<VehicleData> vehicleData = new List<VehicleData>();
        foreach (GameObject obj in vehicles)
        {
            if (Application.isPlaying)
            {
                vehicleData.Add(obj.GetComponent<VehicleInfo>().GetVehicleData());
            }
            else
            {
#if UNITY_EDITOR
                vehicleData.Add(obj.GetComponent<VehicleEditor>().GetVehicleData());
#endif
            }
        }
        return vehicleData;
    }

    public List<VehicleData> GetLevelData(int levelIndex, string path = "", bool doDecrypt = true)
    {
        return SaveJsonHandler.GetList($"level_{levelIndex + 1}_default.json", path, doDecrypt);
    }
}