using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VehicleEditor;

public class VehicleRefreshManager : MonoBehaviour
{
    public List<GameObject> vehiclePrefabsLength1 = new List<GameObject>();
    public List<GameObject> vehiclePrefabsLength2 = new List<GameObject>();
    public List<GameObject> vehiclePrefabsLength3 = new List<GameObject>();

    public GameObject vehiclePrefabMain;
    public GameObject vehiclePrefabLength1;
    public GameObject vehiclePrefabLength2;
    public GameObject vehiclePrefabLength3;

    public void RefreshVehiclesOnBoard(GameObject vehicle)
    {
        if (Application.isPlaying)
        {
            VehicleInfo vehicleInfo = vehicle.GetComponent<VehicleInfo>();
            Destroy(vehicle.GetComponent<VehicleEditor>());

            AdjustObjectFromCurrentSettings(vehicle, vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, vehicleInfo.isMainVehicle);
            AdjustChildFromCurrentSettings(vehicle, (VehicleLength)vehicleInfo.vehicleLength, vehicleInfo.isFacingBackwards, vehicleInfo.isHorizontal, vehicleInfo.isMainVehicle); // vehicleInfo.selectedPrefabIndex,
        }
        else
        {
        #if UNITY_EDITOR
            VehicleEditor vehicleEditor = vehicle.GetComponent<VehicleEditor>();

            AdjustObjectFromCurrentSettings(vehicle, vehicleEditor.vehicleLength, vehicleEditor.isHorizontal, vehicleEditor.isMainVehicle);
            AdjustChildFromCurrentSettings(vehicle, (VehicleLength)vehicleEditor.vehicleLength, vehicleEditor.isFacingBackwards, vehicleEditor.isHorizontal, vehicleEditor.isMainVehicle); // vehicleEditor.selectedPrefabIndex,
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
        }

        if (Application.isPlaying)
        {
            Destroy(vehicle.GetComponent<MeshRenderer>());
        }
    }

    private void AdjustChildFromCurrentSettings(GameObject vehicle, VehicleLength vehicleLength, bool isFacingBackwards, bool isHorizontal, bool isMainVehicle)
    {
        // Korábbi gyermek objektumok törlése
        for (int i = vehicle.transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(vehicle.transform.GetChild(i).gameObject);
            else
            {
                #if UNITY_EDITOR
                    DestroyImmediate(vehicle.transform.GetChild(i).gameObject);
                #endif
            }
        }

        // Megfelelõ prefab kiválasztása
        GameObject prefabToSpawn = null;

        switch ((int)vehicleLength)
        {
            case 1:
                prefabToSpawn = vehiclePrefabLength1;
                break;

            case 2:
                prefabToSpawn = isMainVehicle ? vehiclePrefabMain : vehiclePrefabLength2;
                break;

            case 3:
                prefabToSpawn = vehiclePrefabLength3;
                break;
            default:
                Debug.LogError("Prefab nincs beállítva a megadott hosszra!");
                return;
        }

        if (prefabToSpawn != null)
        {
            // Új gyermek objektum létrehozása
            GameObject childObject = Instantiate(prefabToSpawn, vehicle.transform);
            childObject.name = prefabToSpawn.name;

            // Renderer alapján méret meghatározása
            Renderer childRenderer = childObject.GetComponentInChildren<Renderer>();
            Renderer parentRenderer = vehicle.GetComponentInChildren<Renderer>();
            if (childObject.name != "SM_Veh_Scooter")
            {
                if (childRenderer != null && parentRenderer != null)
                {
                    Vector3 childSize = childRenderer.bounds.size;
                    Vector3 parentSize = parentRenderer.bounds.size;

                    Vector3 scaleRatio;

                    if (isHorizontal)
                    {
                        // horizontális esetben igazítjuk az értékeket
                        scaleRatio = new Vector3(
                            parentSize.z / childSize.z,
                            parentSize.y / childSize.y,
                            parentSize.x / childSize.x
                        );
                    }
                    else
                    {
                        // vertikális esetben normál méretezés
                        scaleRatio = new Vector3(
                            parentSize.x / childSize.x,
                            parentSize.y / childSize.y,
                            parentSize.z / childSize.z
                        );
                    }
                    childObject.transform.localScale = scaleRatio;
                }
                else
                {
                    Debug.LogWarning("Nem található renderer a méretezéshez!");
                    childObject.transform.localScale = Vector3.one;
                }
            }
            else // Ha az object neve "SM_Veh_Scooter"
            {
                childObject.transform.localScale = new Vector3(1, 1, 0.7f);
            }

            childObject.transform.localPosition = Vector3.zero - new Vector3(0, 0.5f, 0);
            childObject.transform.localRotation = Quaternion.Euler(0, isFacingBackwards ? 180f : 0f, 0);
        }
    } // int selectedPrefabIndex,

    public void magicItemReduce(GameObject vehicle)
    {
        VehicleInfo vehicleInfo = vehicle.GetComponent<VehicleInfo>();
        if (vehicleInfo.vehicleLength == 1)
        {
            Destroy(vehicle);
        }
        else
        {
            vehicleInfo.vehicleLength -= 1;
            AdjustObjectFromCurrentSettings(vehicle, vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, false);
            AdjustChildFromCurrentSettings(vehicle, (VehicleLength)vehicleInfo.vehicleLength, vehicleInfo.isFacingBackwards, vehicleInfo.isHorizontal, false);
            vehicle.GetComponent<VehicleController>().OnTouchEnded(); // Az adatok frissítése érdekében
        }
    }

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