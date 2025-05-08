using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class raycast1 : MonoBehaviour
{
    private void Update()
    {
        GetMaxMovingDistances();    
    }

    void GetMaxMovingDistances()
    {
        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit hitForward;
        RaycastHit hitBackward;

        Physics.Raycast(start, direction, out hitForward, 7f);
        Physics.Raycast(start, -direction, out hitBackward, 7f);

        Debug.DrawRay(start, direction * 7f, Color.red);
        Debug.DrawRay(start, -direction * 7f, Color.red);
        Debug.Log($"Hátra: {hitBackward.distance} | Elõre: {hitForward.distance}");
    }
}