using UnityEngine;

// Yan kameranın hedef objeyi X ve Y ekseninde
// yumuşak bir şekilde takip etmesini sağlayan kamera scripti.

public class SideCameraFollow : MonoBehaviour
{
    public Transform target;

    public float fixedZ = -10f;

    public float xSmoothTime = 0.08f;
    public float ySmoothTime = 0.12f;

    private float xVelocity;
    private float yVelocity;

    // Kamera pozisyonunu hedefe göre geç ve yumuşak şekilde günceller
    private void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = transform.position;

        pos.x = Mathf.SmoothDamp(
            pos.x,
            target.position.x,
            ref xVelocity,
            xSmoothTime
        );

        pos.y = Mathf.SmoothDamp(
            pos.y,
            target.position.y,
            ref yVelocity,
            ySmoothTime
        );

        pos.z = fixedZ;

        transform.position = pos;
    }
}
