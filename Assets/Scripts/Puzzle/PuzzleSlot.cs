using UnityEngine;
using UnityEngine.UI;

// Puzzle oyununda parçaların bırakıldığı slotları
// ve doğru eşleşme görsel efektlerini yöneten sınıf.

public class PuzzleSlot : MonoBehaviour
{
    public int slotId;
    public GameObject correctVfxPrefab;

    [Header("Highlight")]
    public Image slotImage;

    // Slot üzerindeki Image referansını otomatik alır
    private void Awake()
    {
        if (slotImage == null)
            slotImage = GetComponent<Image>();
    }

    // Doğru parça yerleştirildiğinde VFX oynatır
    public void PlayCorrectVFX()
    {
        if (correctVfxPrefab == null) return;

        GameObject vfx = Instantiate(correctVfxPrefab, transform);
        vfx.transform.SetAsLastSibling();
        vfx.transform.localPosition = Vector3.zero;

        Destroy(vfx, 1f);
    }
}
