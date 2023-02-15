using UnityEngine;
using DG.Tweening;

public class SelectUI : MonoBehaviour
{
    private float timeToAppear = 0.5f;

    public Transform[] transforms;
    private Vector3[] cachedPositions;
    private Vector3[] cachedScales;

    private bool isAppearing = false;
    private bool isDisappearing = false;

    void Start()
    {
        if (transforms.Length == 0)
            return;
        cachedPositions = new Vector3[transforms.Length];
        cachedScales = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            cachedPositions[i] = transforms[i].localPosition;
            cachedScales[i] = transforms[i].localScale;
        }

        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localScale = Vector3.zero;
            transforms[i].localPosition = Vector3.zero;
        }
    }

    void OnDestroy() {
        KillTweens();
        transforms = new Transform[0];
        cachedPositions = new Vector3[0];
        cachedScales = new Vector3[0];
    }

    private void KillTweens() {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOKill();
        }
    }

    public void Appear() {
        if (isAppearing)
            return;
        isAppearing = true;
        isDisappearing = false;
        KillTweens();
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(cachedScales[i], timeToAppear);
            transforms[i].DOLocalMove(cachedPositions[i], timeToAppear);
        }
    }

    public void Disappear() {
        if (isDisappearing)
            return;
        isDisappearing = true;
        isAppearing = false;
        KillTweens();
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(Vector3.zero, timeToAppear);
            transforms[i].DOLocalMove(Vector3.zero, timeToAppear);
        }
    }
}
