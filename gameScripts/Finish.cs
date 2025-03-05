using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    //public GameManager GameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainVehicle"))
        {
            collision.gameObject.GetComponent<IndicateAlign>().DestroyIndicateAligns();
            Destroy(collision.gameObject);
            
            //GameManager.GameFinished();
        }
    }
}
