using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectUI : MonoBehaviour
{
    private float scaleX;
    private float scaleY;
    private float scaleZ;

    private float timeToScale = 0.5f;

    void Start()
    {
        scaleX = this.transform.localScale.x;
        scaleY = this.transform.localScale.y;
        scaleZ = this.transform.localScale.z;

        this.transform.localScale = Vector3.zero;
    }

    void OnDestroy() {
        this.transform.DOKill();
    }

    public void Appear() {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(new Vector3(scaleX, scaleY, scaleZ), timeToScale);
    }

    public void Disappear() {
        this.transform.DOScale(Vector3.zero, timeToScale);
    }
}
