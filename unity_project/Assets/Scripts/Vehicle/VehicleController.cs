using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private Vector3 lastPosition; // Az előző frame pozíciója
    private Vector3 beforePosition; // A húzás megkezdése előtti pozíció

    private VehicleInfo vehicleInfo;
    private IndicateAlign indicateAlign;
    private BoardManager boardManager;
    private InGameUIManager inGameUIManager;

    private Tuple<float, float> maxMovingDistance; // A jármű által maximálisan megtehető távolság <előre, hátra>
    private Vector3 currentTargetPosition; // Az aktuális célpozíció
    private float maxDragSpeed = 50f; // Maximális húzási sebesség (egység/frame)

    [Obsolete]
    void Start()
    {
        // Megkeressük az objektumon lévő VehicleInfo komponenst
        vehicleInfo = GetComponent<VehicleInfo>();
        indicateAlign = GetComponent<IndicateAlign>();
        boardManager = FindObjectOfType<BoardManager>();
        inGameUIManager = FindObjectOfType<InGameUIManager>();

        lastPosition = transform.position; // Kezdeti érték beállítása
        beforePosition = transform.position;
    }

    public void OnTouchBegan()
    {
        //Debug.Log("Jármű mozgatásának kezdete.");
        maxMovingDistance = GetMaxMovingDistances(); // Az új maximális távolság beállítása
        //Debug.Log(maxMovingDistance);
        indicateAlign.SpawnIndicateAligns();
    }

    public void OnTouchMoved(Vector3 touchPosition)
    {
        //Debug.Log(touchPosition);
        indicateAlign.MoveIndicateAligns();

        // Ellenőrizzük, hogy vízszintes vagy függőleges mozgásra van-e szükség a VehicleInfo alapján
        if (vehicleInfo.isHorizontal)
        {
            currentTargetPosition = new Vector3(touchPosition.x, transform.position.y, transform.position.z);
        }
        else
        {
            currentTargetPosition = new Vector3(transform.position.x, transform.position.y, touchPosition.z);
        }

        // Sebesség számítása
        Vector3 velocity = (currentTargetPosition - lastPosition) / Time.deltaTime;

        // Sebesség korlátozása
        if (velocity.magnitude > maxDragSpeed)
        {
            velocity = velocity.normalized * maxDragSpeed;
            currentTargetPosition = lastPosition + velocity * Time.deltaTime;
            //Debug.Log(velocity);
        }

        // Korlátozzuk, hogy meddig lehet mozgatni
        if (vehicleInfo.isHorizontal)
        {
            if (currentTargetPosition.x > beforePosition.x + maxMovingDistance.Item1)
            {
                currentTargetPosition.x = beforePosition.x + maxMovingDistance.Item1;
            }
            if (currentTargetPosition.x < beforePosition.x - maxMovingDistance.Item2)
            {
                currentTargetPosition.x = beforePosition.x - maxMovingDistance.Item2;
            }
        }
        else
        {
            if (currentTargetPosition.z > beforePosition.z + maxMovingDistance.Item1)
            {
                currentTargetPosition.z = beforePosition.z + maxMovingDistance.Item1;
            }
            if (currentTargetPosition.z < beforePosition.z - maxMovingDistance.Item2)
            {
                currentTargetPosition.z = beforePosition.z - maxMovingDistance.Item2;
            }
        }
        
        
        transform.position = currentTargetPosition;

        // Frissítjük az előző pozíciót
        lastPosition = transform.position;
    }

    public void OnTouchEnded()
    {
        //Debug.Log("Jármű mozgatásának vége.");

        indicateAlign.DestroyIndicateAlign();
        AlignToGrid();
        //board.RefreshBoardState(); ---------------------------------------- trigger digger skibidi ni

        if (CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, beforePosition) != CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, currentTargetPosition))
        {
            //Debug.Log($"Változás történt: | {CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, beforePosition)} | ---> | {CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, currentTargetPosition)} |"); // Jelez, ha elmozdult valamelyik jármű
        }

        // Frissítjük az előző pozíciót
        Vector3 calculatedAlign = CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, transform.position);
        if (calculatedAlign != beforePosition) // Ha a jármű új mezőre kerül
        {
            inGameUIManager.UpdateVehicleMoves();
        }
        lastPosition = calculatedAlign;
        beforePosition = calculatedAlign;
    }

    public Tuple<float, float> GetMaxMovingDistances()
    {
        Vector3 start = transform.position;
        start.y = 0.65f;
        Vector3 direction = transform.forward;
        RaycastHit hitForward;
        RaycastHit hitBackward;

        int layerMask = LayerMask.GetMask("Vehicles", "Walls");

        Physics.Raycast(start, direction, out hitForward, 10f, layerMask); // 10 a MainCar miatt
        Physics.Raycast(start, -direction, out hitBackward, 7f, layerMask);

        //Debug.DrawRay(start, direction * 10f, Color.red);
        //Debug.DrawRay(start, -direction * 7f, Color.red);

        if (vehicleInfo.vehicleLength % 2 == 0)
        {
            //Debug.Log($"{hitForward.distance - vehicleInfo.vehicleLength / 2} | {hitBackward.distance - vehicleInfo.vehicleLength / 2}");
            return new Tuple<float, float>(hitForward.distance - vehicleInfo.vehicleLength / 2, hitBackward.distance - vehicleInfo.vehicleLength / 2);
        }
        else
        {
            //Debug.Log($"{Mathf.Floor(hitForward.distance - vehicleInfo.vehicleLength / 2)} | {Mathf.Floor(hitBackward.distance - vehicleInfo.vehicleLength / 2)}");
            return new Tuple<float, float>(Mathf.Floor(hitForward.distance - vehicleInfo.vehicleLength / 2), Mathf.Floor(hitBackward.distance - vehicleInfo.vehicleLength / 2));
        }
    }

    public void AlignToGrid()
    {
        // Lerp segítségével simítjuk a mozgást
        StartCoroutine(SmoothMove(CalculateAlign(vehicleInfo.vehicleLength, vehicleInfo.isHorizontal, transform.position), 0.2f)); // 0.2 másodperc alatt mozgatja a célpozícióra
    }

    public Vector3 CalculateAlign(int vehicleLength, bool isHorizontal, Vector3 currentPosition)
    {
        Vector3 targetPosition = transform.position;

        if (vehicleLength % 2 == 0) // Ha páros a jármű hossza
        {
            if (isHorizontal)
            {
                if ((Math.Abs((Mathf.Round(currentPosition.x) - 0.5f) - currentPosition.x) < Math.Abs(Mathf.Round(currentPosition.x) + 0.5f) - currentPosition.x))
                {
                    targetPosition = new Vector3(Convert.ToSingle(Mathf.Round(currentPosition.x)) - 0.5f, currentPosition.y, currentPosition.z);
                }
                else
                {
                    targetPosition = new Vector3(Convert.ToSingle(Mathf.Round(currentPosition.x)) + 0.5f, currentPosition.y, currentPosition.z);
                }
            }
            else
            {
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, Mathf.Round(currentPosition.z));
            }
        }
        else // Ha páratlan a jármű hossza
        {
            if (isHorizontal)
            {
                targetPosition = new Vector3(Mathf.Round(currentPosition.x), currentPosition.y, currentPosition.z);
            }
            else
            {
                if (Math.Abs((Convert.ToSingle(Mathf.Round(currentPosition.z)) - 0.5f) - currentPosition.z) < Math.Abs((Convert.ToSingle(Mathf.Round(currentPosition.z)) + 0.5f) - currentPosition.z))
                {
                    targetPosition = new Vector3(currentPosition.x, currentPosition.y, Convert.ToSingle(Mathf.Round(currentPosition.z)) - 0.5f);
                }
                else
                {
                    targetPosition = new Vector3(currentPosition.x, currentPosition.y, Convert.ToSingle(Mathf.Round(currentPosition.z)) + 0.5f);
                }
            }
        }

        return targetPosition;
    }

    private IEnumerator SmoothMove(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Biztosítjuk, hogy pontosan a célpozícióra kerüljön
    }
}
