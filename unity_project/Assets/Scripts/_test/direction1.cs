using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class direction1 : MonoBehaviour
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

        Physics.Raycast(start, direction, out hitForward, 3f);
        Physics.Raycast(start, -direction, out hitBackward, 3f);

        Debug.DrawRay(start, direction * 3f, Color.blue);
        Debug.DrawRay(start, -direction * 3f, Color.white);

        //Debug.Log(transform.eulerAngles.y);
        //Debug.Log(direction);
    }
}
