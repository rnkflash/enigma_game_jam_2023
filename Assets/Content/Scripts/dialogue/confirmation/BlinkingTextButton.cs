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

    private float blinkingTime = 0.5f;

    void Awake() {
        buttonText = GetComponentInChildren<TMP_Text>();
        bg = GetComponent<RawImage>();
        StopBlinking();
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
        DOTween.Kill(bg);
        SetAlpha(0.0f);
    }

    private void SetAlpha(float alpha) {
        Color currColor = bg.color;
        currColor.a = alpha;
        bg.color = currColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartBlinking();
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
        buttonText.text = text;
    }
}