using UnityEngine;

// UI üzerinde gösterilen sandık modelinin
// sürekli kendi etrafında dönmesini sağlayan görsel script.

public class UIChestModelRotator : MonoBehaviour
{
    public float rotateSpeed = 30f;

    // UI sandık modelini her karede y ekseninde döndürür
    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
