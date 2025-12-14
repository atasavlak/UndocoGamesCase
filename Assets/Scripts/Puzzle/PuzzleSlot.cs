using UnityEngine;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour
{
    public int slotId;
    public GameObject correctVfxPrefab;


    [Header("Highlight")]
    public Image slotImage;

    private void Awake()
    {
        if (slotImage == null)
            slotImage = GetComponent<Image>();
    }

    public void PlayCorrectVFX()
    {
        if (correctVfxPrefab == null) return;

        GameObject vfx = Instantiate(correctVfxPrefab, transform);
        vfx.transform.SetAsLastSibling();
        vfx.transform.localPosition = Vector3.zero;

        Destroy(vfx, 1f);
    }

}
