using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

    private Tween snapTween;
    private Tween scaleTween;
    private Tween moveTween;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging || !enabled) return;
        TweenScale(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging || !enabled) return;
        TweenScale(normalScale);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enabled) return;

        isDragging = true;

        snapTween?.Kill();
        moveTween?.Kill();

        rect.SetParent(canvas.transform, true);
        TweenScale(hoverScale);

        if (img != null) img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

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
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = targetSize;
                rect.localScale = Vector3.one;
                enabled = false;
            });
    }

    private void ResetPieceTween()
    {
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

    private void TweenScale(float target)
    {
        scaleTween?.Kill();
        scaleTween = rect
            .DOScale(Vector3.one * target, scaleTweenDuration)
            .SetEase(Ease.OutCubic);
    }
}
