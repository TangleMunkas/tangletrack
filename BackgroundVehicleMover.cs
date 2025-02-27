using UnityEngine;

public class BackgroundVehicleMover : MonoBehaviour
{
    public Vector3 startPosition; // Kezd�poz�ci�
    public Vector3 endPosition; // C�lpoz�ci�
    public float speed = 5f; // Sebess�g (m/s)

    private void Start()
    {
        transform.position = startPosition; // Kezdeti poz�ci� be�ll�t�sa
    }

    void Update()
    {
        // Mozgatja az aut�t a c�l fel�
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

        // Ha el�rte a c�lpoz�ci�t, visszaugrik a kezd�poz�ci�ra
        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            transform.position = startPosition;
        }
    }
}
