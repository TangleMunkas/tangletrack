using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //List<Vector2> VehicleInfoList = new List<Vector2>(); // Inicializáljuk a listát

    public int gridSize = 6; // Rács mérete (6x6)
    //public bool[,] cellStatus; // A rács celláinak foglaltsági állapota
    public bool isDraggingVehicle = false;
    /*
    void Start()
    {
        // Inicializáljuk a rácsot és beállítjuk az összes cellát üresre (false)
        cellStatus = new bool[gridSize, gridSize];
        InitializeGrid();
    }

    public void RefreshBoardState()
    {
        GetBoardState();
        UpdateBoardState();
    }

    void InitializeGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                cellStatus[x, y] = false; // Kezdetben minden cella üres
            }
        }
    }

    public void GetBoardState()
    {
        // Megkeressük az összes "Vehicle" taggel rendelkezõ objektumot
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        // Töröljük a VehicleInfoList tartalmát, hogy ne legyenek duplikációk
        VehicleInfoList.Clear();

        // Végigmegyünk minden "Vehicle" taggel rendelkezõ objektumon
        foreach (GameObject vehicle in vehicles)
        {
            // Hozzáadjuk a jármû Cell listájának tartalmát a VehicleInfoList-hez
            VehicleInfo vehicleInfo = vehicle.GetComponent<VehicleInfo>();
            if (vehicleInfo != null)
            {
                VehicleInfoList.AddRange(vehicleInfo.Cells);
            }
        }
    }

    public void UpdateBoardState()
    {
        InitializeGrid();

        foreach (Vector2 position in VehicleInfoList)
        {
            int x = Convert.ToInt32(position.x - 0.5);
            int y = Convert.ToInt32(position.y - 0.5);
            try { cellStatus[x, y] = true; } catch { };   
        }
    }
    */
}
