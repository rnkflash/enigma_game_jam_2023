using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using static UnityEngine.EventSystems.PointerEventData;
using TMPro;

public class BlinkingTextButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private RawImage bg;
    private TMP_Text buttonText;
    public event Action onClickListeners;

    public interface IBlinkingButtonParent {
        void MouseSelected(BlinkingTextButton button);
    }

    private IBlinkingButtonParent parent = null;

    private float blinkingTime = 0.5f;

    void Awake() {
        buttonText = GetComponentInChildren<TMP_Text>();
        bg = GetComponent<RawImage>();
        StopBlinking();
    }

    public void SetParent(IBlinkingButtonParent newParent) {
        parent = newParent;
    }

    void OnDestroy() {
        DOTween.Kill(bg);
    }

    public void StartBlinking() {
        DOTween.Kill(bg);
        SetAlpha(0.05f);
        bg.DOFade(0.0f, blinkingTime).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopBlinking() {
        if (bg != null) {
            DOTween.Kill(bg);
            SetAlpha(0.0f);
        }
    }

    private void SetAlpha(float alpha) {
        Color currColor = bg.color;
        currColor.a = alpha;
        bg.color = currColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartBlinking();
        parent.MouseSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopBlinking();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == InputButton.Left) {
            onClickListeners?.Invoke();
        }
    }

    public void SetText(string text) {
        if (buttonText != null)
            buttonText.text = text;
    }
}