using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleRoute
{
    public Transform spawnPoint;
    public Transform endPoint;
}

public class InGameBackgroundVehicleManager : MonoBehaviour
{
    [Header("Prefabok és pályák")]
    public List<GameObject> vehiclePrefabs;
    public List<VehicleRoute> routes;

    [Header("Sebesség és spawn idõ")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;
    public float vehicleSpeed = 5f;

    [Header("Közös szülõobjektum")]
    public Transform vehicleContainer; // InGameBackgroundVehicleContainer

    private List<Coroutine> activeCoroutines = new();

    private void Start()
    {
        HandlePerformanceMode();
    }

    public void HandlePerformanceMode()
    {
        if (SettingsManager.isLowPerformanceModeOn)
        {
            // Kikapcsoljuk a jármûrendszert
            StopAllCoroutines();
            foreach (Transform child in vehicleContainer)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            // Elindítjuk a jármû spawnolást
            foreach (var route in routes)
            {
                Coroutine c = StartCoroutine(SpawnLoop(route));
                activeCoroutines.Add(c);
            }
        }
    }

    IEnumerator SpawnLoop(VehicleRoute route)
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];
            GameObject vehicle = Instantiate(prefab, route.spawnPoint.position, route.spawnPoint.rotation, vehicleContainer);
            vehicle.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            vehicle.AddComponent<InGameBackgroundVehicleMover>().Init(route.endPoint, vehicleSpeed);
        }
    }
}
