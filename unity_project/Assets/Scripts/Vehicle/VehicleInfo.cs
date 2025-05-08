using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInfo : MonoBehaviour
{
    private VehicleRefreshManager vehicleRefreshManager;

    [HideInInspector] public int vehicleLength = 2;
    //[HideInInspector] public int selectedPrefabIndex = 0;
    [HideInInspector] public bool isHorizontal = false;
    [HideInInspector] public bool isFacingBackwards = false;
    [HideInInspector] public bool isMainVehicle = false;

    [HideInInspector] public bool isDraggingRN = false;

    //public List<Vector2> Cells; // A jármû által elfoglalt cellák listája
    //Vector2 vehiclePosition = new Vector2();

    private void Awake()
    {
        vehicleRefreshManager = GameObject.FindWithTag("VehicleRefreshManager").GetComponent<VehicleRefreshManager>();
    }

    public VehicleData GetVehicleData()
    {
        VehicleData vehicleData = new VehicleData
        {
            positionX = transform.position.x,
            positionY = transform.position.y,
            positionZ = transform.position.z,

            vehicleLength = vehicleLength,
            //selectedPrefabIndex = selectedPrefabIndex,
            isHorizontal = isHorizontal,
            isFacingBackwards = isFacingBackwards,
            isMainVehicle = isMainVehicle
        };

        return vehicleData;
    }

    public void SetVehicleData(VehicleData vehicleData)
    {
        transform.position = new Vector3(vehicleData.positionX, vehicleData.positionY, vehicleData.positionZ);

        vehicleLength = vehicleData.vehicleLength;
        //selectedPrefabIndex = vehicleData.selectedPrefabIndex;
        isHorizontal = vehicleData.isHorizontal;
        isFacingBackwards = vehicleData.isFacingBackwards;
        isMainVehicle = vehicleData.isMainVehicle;

        vehicleRefreshManager.RefreshVehiclesOnBoard(gameObject);
    }


    /*
    private void Awake()
    {
        AlignCells();
    }

    public void UpdateAlignCells()
    {
        AlignCells();
    }

    void AlignCells()
    {
        vehiclePosition.x = gameObject.transform.position.z;
        vehiclePosition.y = gameObject.transform.position.x;

        if (vehicleLength % 2 != 0) // Ha páratlan a jármû hossza
        {
            if (isHorizontal)
            {
                for (int i = 0; i < vehicleLength; i++)
                {
                    Vector2 cell = Cells[i];
                    cell.x = vehiclePosition.x + (-((vehicleLength - 1) / 2) + i);
                    cell.y = 3;
                    Cells[i] = cell;
                }
            }

            else
            {
                for (int i = 0; i < vehicleLength; i++)
                {
                    Vector2 cell = Cells[i];
                    cell.y = vehiclePosition.y + (-((vehicleLength - 1) / 2) + i);
                    cell.x = 3;
                    Cells[i] = cell;
                }
            }
        }

        else // Ha páros a jármû hossza
        {
            if (isHorizontal)
            {
                for (int i = 0; i < vehicleLength; i++)
                {
                    Vector2 cell = Cells[i];
                    cell.x = vehiclePosition.x + (-(vehicleLength / 2) + i) + 0.5f;
                    cell.y = 3;
                    Cells[i] = cell;
                }
            }

            else
            {
                for (int i = 0; i < vehicleLength; i++)
                {
                    Vector2 cell = Cells[i];
                    cell.y = vehiclePosition.y + (-(vehicleLength / 2) + i) + 0.5f;
                    cell.x = 3;
                    Cells[i] = cell;
                }
            }
        }
    } */
}