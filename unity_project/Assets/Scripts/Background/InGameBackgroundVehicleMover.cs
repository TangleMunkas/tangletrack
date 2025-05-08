using UnityEngine;

public class InGameBackgroundVehicleMover : MonoBehaviour
{
    private Transform target;
    private float speed;

    public void Init(Transform targetPoint, float moveSpeed)
    {
        target = targetPoint;
        speed = moveSpeed;
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}