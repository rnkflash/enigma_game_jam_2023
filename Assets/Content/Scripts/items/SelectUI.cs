using UnityEngine;
using DG.Tweening;

public class SelectUI : MonoBehaviour
{
    private float timeToAppear = 0.5f;

    public Transform[] transforms;
    private Vector3[] cachedPositions;
    private Vector3[] cachedScales;

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
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOKill();
        }
        transforms = new Transform[0];
        cachedPositions = new Vector3[0];
        cachedScales = new Vector3[0];
    }

    public void Appear() {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(cachedScales[i], timeToAppear);
            transforms[i].DOLocalMove(cachedPositions[i], timeToAppear);
        }
    }

    public void Disappear() {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(Vector3.zero, timeToAppear);
            transforms[i].DOLocalMove(Vector3.zero, timeToAppear);
        }
    }
}
