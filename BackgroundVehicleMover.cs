using UnityEngine;

public class BackgroundVehicleMover : MonoBehaviour
{
    public Vector3 startPosition; // Kezdõpozíció
    public Vector3 endPosition; // Célpozíció
    public float speed = 5f; // Sebesség (m/s)

    private void Start()
    {
        transform.position = startPosition; // Kezdeti pozíció beállítása
    }

    void Update()
    {
        // Mozgatja az autót a cél felé
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

        // Ha elérte a célpozíciót, visszaugrik a kezdõpozícióra
        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            transform.position = startPosition;
        }
    }
}
