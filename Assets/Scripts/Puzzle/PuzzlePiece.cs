using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

// Puzzle parçasının sürükleme, bırakma, slot kontrolü
// ve görsel geri bildirim (scale / snap) davranışlarını yönetir.

public class PuzzlePiece : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public int pieceId;

    public float normalScale = 0.85f;
    public float hoverScale = 1.05f;

    public float scaleTweenDuration = 0.12f;

    public float snapDuration = 0.18f;
    public float snapScalePunch = 0.12f;

    public float resetMoveDuration = 0.22f;
    private Ease resetMoveEase = Ease.OutCubic;

    private RectTransform rect;
    private Canvas canvas;
    private Image img;

    private Transform startParent;
    private Vector2 startAnchoredPos;
    private Vector3 startWorldPos;

    private bool isDragging;
    private bool isPlaced = false;

    private Tween snapTween;
    private Tween scaleTween;
    private Tween moveTween;

    // Başlangıç referanslarını ve varsayılan scale değerini ayarlar
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        img = GetComponent<Image>();

        startParent = rect.parent;
        startAnchoredPos = rect.anchoredPosition;
        startWorldPos = rect.position;

        rect.localScale = Vector3.one * normalScale;
    }

    // Mouse parça üzerine geldiğinde hover scale uygular
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging || isPlaced) return;
        TweenScale(hoverScale);
    }

    // Mouse parça üzerinden çıkınca normal scale'e döner
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging || isPlaced) return;
        TweenScale(normalScale);
    }

    // Sürükleme başladığında parçayı canvas altına alır
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        isDragging = true;

        snapTween?.Kill();
        moveTween?.Kill();

        rect.SetParent(canvas.transform, true);
        TweenScale(hoverScale);

        if (img != null) img.raycastTarget = false;
    }

    // Sürükleme sırasında parçayı mouse ile birlikte hareket ettirir
    public void OnDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // Sürükleme bitince slot kontrolü yapar
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!enabled) return;

        isDragging = false;

        if (img != null) img.raycastTarget = true;

        PuzzleSlot slot = null;
        if (eventData.pointerEnter != null)
            slot = eventData.pointerEnter.GetComponentInParent<PuzzleSlot>();

        if (slot != null && slot.slotId == pieceId)
        {
            slot.PlayCorrectVFX();
            PuzzleGameController.Instance.PlayCorrectSfx();

            SnapToSlot(slot.transform);
            PuzzleGameController.Instance.NotifyCorrectPlacement();
            return;
        }

        ResetPieceTween();
    }

    // Parçayı doğru slota animasyonlu şekilde sabitler
    private void SnapToSlot(Transform slotTransform)
    {
        RectTransform slotRect = slotTransform as RectTransform;

        snapTween?.Kill();
        scaleTween?.Kill();
        moveTween?.Kill();

        rect.SetParent(slotTransform, true);

        Vector2 targetSize = rect.sizeDelta;
        if (slotRect != null)
            targetSize = slotRect.sizeDelta;

        rect.localScale = Vector3.one;

        snapTween = DOTween.Sequence()
            .Append(rect.DOAnchorPos(Vector2.zero, snapDuration).SetEase(Ease.OutCubic))
            .Join(rect.DOSizeDelta(targetSize, snapDuration).SetEase(Ease.OutCubic))
            .Join(rect.DOPunchScale(Vector3.one * snapScalePunch, snapDuration, 6, 0.9f))
            .OnComplete(() =>
            {
                snapTween?.Kill();
                scaleTween?.Kill();
                moveTween?.Kill();

                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = targetSize;
                rect.localScale = Vector3.one;

                isPlaced = true;
                enabled = false;
            });
    }

    // Yanlış bırakıldığında parçayı başlangıç konumuna geri taşır
    private void ResetPieceTween()
    {
        if (isPlaced) return;

        snapTween?.Kill();
        moveTween?.Kill();

        rect.SetParent(canvas.transform, true);

        moveTween = rect
            .DOMove(startWorldPos, resetMoveDuration)
            .SetEase(resetMoveEase)
            .OnComplete(() =>
            {
                rect.SetParent(startParent, false);
                rect.anchoredPosition = startAnchoredPos;
                TweenScale(normalScale);
            });

        TweenScale(normalScale);
    }

    // Scale animasyonunu kontrollü şekilde oynatır
    private void TweenScale(float target)
    {
        scaleTween?.Kill();
        scaleTween = rect
            .DOScale(Vector3.one * target, scaleTweenDuration)
            .SetEase(Ease.OutCubic);
    }
}
