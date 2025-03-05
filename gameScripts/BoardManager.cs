using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //List<Vector2> VehicleInfoList = new List<Vector2>(); // Inicializ�ljuk a list�t

    public int gridSize = 6; // R�cs m�rete (6x6)
    //public bool[,] cellStatus; // A r�cs cell�inak foglalts�gi �llapota
    public bool isDraggingVehicle = false;
    /*
    void Start()
    {
        // Inicializ�ljuk a r�csot �s be�ll�tjuk az �sszes cell�t �resre (false)
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
                cellStatus[x, y] = false; // Kezdetben minden cella �res
            }
        }
    }

    public void GetBoardState()
    {
        // Megkeress�k az �sszes "Vehicle" taggel rendelkez� objektumot
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        // T�r�lj�k a VehicleInfoList tartalm�t, hogy ne legyenek duplik�ci�k
        VehicleInfoList.Clear();

        // V�gigmegy�nk minden "Vehicle" taggel rendelkez� objektumon
        foreach (GameObject vehicle in vehicles)
        {
            // Hozz�adjuk a j�rm� Cell list�j�nak tartalm�t a VehicleInfoList-hez
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
