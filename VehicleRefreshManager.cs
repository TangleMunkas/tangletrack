using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VehicleEditor;

public class VehicleRefreshManager : MonoBehaviour
{
    public List<GameObject> vehiclePrefabsLength1 = new List<GameObject>();
    public List<GameObject> vehiclePrefabsLength2 = new List<GameObject>();
    public List<GameObject> vehiclePrefabsLength3 = new List<GameObject>();


    public void RefreshVehiclesOnBoard(GameObject vehicle)
    {
        if (Application.isPlaying)
        {
            VehicleInfo vehicleInfo = vehicle.GetComponent<VehicleInfo>();
            Destroy(vehicle.GetComponent<MeshRenderer>());
            Destroy(vehicle.GetComponent<VehicleEditor>());

            AdjustObjectFromCurrentSettings(vehicle, vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, vehicleInfo.isMainVehicle);
            AdjustChildFromCurrentSettings(vehicle, (VehicleLength)vehicleInfo.vehicleLength, vehicleInfo.isFacingBackwards); // vehicleInfo.selectedPrefabIndex,
        }
        else
        {
        #if UNITY_EDITOR
            VehicleEditor vehicleEditor = vehicle.GetComponent<VehicleEditor>();
            DestroyImmediate(vehicle.GetComponent<MeshRenderer>());

            AdjustObjectFromCurrentSettings(vehicle, vehicleEditor.vehicleLength, vehicleEditor.isHorizontal, vehicleEditor.isMainVehicle);
            AdjustChildFromCurrentSettings(vehicle, (VehicleLength)vehicleEditor.vehicleLength, vehicleEditor.isFacingBackwards); // vehicleEditor.selectedPrefabIndex,
#endif
        }
    }

    private void AdjustObjectFromCurrentSettings(GameObject vehicle, int vehicleLength, bool isHorizontal, bool isMainVehicle)
    {
        vehicle.transform.rotation = Quaternion.Euler(0, isHorizontal ? 90f : 0f, 0);

        Vector3 newPosition = vehicle.transform.position;
        newPosition.y = 1;
        Vector3 newScale = vehicle.transform.localScale;
        newScale.z = vehicleLength - 0.2f;

        if (vehicleLength % 2 == 0)
        {
            if (isHorizontal)
            {
                if (Mathf.Approximately(newPosition.x % 1, 0) && Mathf.Approximately(newPosition.z % 1, 0))
                {
                    newPosition.x += 0.5f;
                    newPosition.z += 0.5f;
                }
            }
            else
            {
                if (!Mathf.Approximately(newPosition.x % 1, 0) && !Mathf.Approximately(newPosition.z % 1, 0))
                {
                    newPosition.x -= 0.5f;
                    newPosition.z -= 0.5f;
                }
            }
        }
        else
        {
            if (isHorizontal)
            {
                if (Mathf.Approximately(newPosition.x % 1, 0) && Mathf.Approximately(newPosition.z % 1, 0))
                {
                    newPosition.z += 0.5f;
                }
            }
            else
            {
                if (Mathf.Approximately(newPosition.x % 1, 0) && Mathf.Approximately(newPosition.z % 1, 0))
                {
                    newPosition.z += 0.5f;
                }
            }
        }

        vehicle.transform.position = newPosition;
        vehicle.transform.localScale = newScale;

        if (isMainVehicle)
        {
            vehicle.gameObject.tag = "MainVehicle";

            Rigidbody rb = vehicle.gameObject.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.freezeRotation = true;
        }
    }

    private void AdjustChildFromCurrentSettings(GameObject vehicle, VehicleLength vehicleLength, bool isFacingBackwards)
    {
        if (vehicle.transform.childCount > 0) // Gyermek objektumok törlése
        {
            for (int i = 0; i < vehicle.transform.childCount; i++)
            {
                if (Application.isPlaying)
                {
                    Destroy(vehicle.transform.GetChild(i));
                }
                else
                {
                #if UNITY_EDITOR
                    DestroyImmediate(vehicle.transform.GetChild(i));
                #endif
                }
            }
        }
        /*
        List<GameObject> selectedList = GetCurrentPrefabList(vehicleLength);

        if (selectedList == null || selectedList.Count == 0)
        {
            Debug.LogWarning($"A(z) {vehicleLength} hosszúsághoz tartozó lista üres. Nem lehet gyermeket létrehozni.");
            return;
        }

        
        GameObject prefabToSpawn = selectedList[Mathf.Clamp(selectedPrefabIndex, 0, selectedList.Count - 1)];

        GameObject childObject = Instantiate(prefabToSpawn); // Új gyermek létrehozása
        childObject.transform.SetParent(vehicle.transform);
        childObject.name = prefabToSpawn.name;

        childObject.transform.localPosition = Vector3.zero;
        childObject.transform.localRotation = Quaternion.Euler(0, isFacingBackwards ? 180f : 0f, 0);
        childObject.transform.localScale = Vector3.one;
        */
    } // int selectedPrefabIndex,

    /*
    public List<GameObject> GetCurrentPrefabList(VehicleLength vehicleLength)
    {
        switch ((VehicleLength)vehicleLength)
        {
            case VehicleLength.Length1:
                return vehiclePrefabsLength1;
            case VehicleLength.Length2:
                return vehiclePrefabsLength2;
            case VehicleLength.Length3:
                return vehiclePrefabsLength3;
            default:
                return null;
        }
    } */
}
/*
        Prefab listák megjelenítése
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vehiclePrefabsLength1"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vehiclePrefabsLength2"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vehiclePrefabsLength3"), true);
*/