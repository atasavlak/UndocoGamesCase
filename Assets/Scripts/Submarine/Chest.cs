using UnityEngine;

// Submarine mini oyununda toplanabilir sandıkların
// idle animasyonu ve toplanma davranışını yöneten sınıf.

public class Chest : MonoBehaviour
{
    public float rotateSpeed = 40f;

    private bool collected = false;

    // Sandığın sahnede sürekli kendi etrafında dönmesini sağlar
    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    // Oyuncu sandığa temas ettiğinde toplanma işlemini yapar
    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            SubmarineGameController.Instance.CollectChest();

            Destroy(gameObject);
        }
    }
}
