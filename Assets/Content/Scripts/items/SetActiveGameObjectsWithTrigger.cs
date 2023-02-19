using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveGameObjectsWithTrigger : MonoBehaviour
{
    public bool setActive;
    public string trigger;
    public GameObject[] targets;
    public bool immediateEffect;

    
    void Start()
    {
        TryToReadTrigger();

        if (immediateEffect) {
            EventBus<TriggerWasSet>.Sub(OnTrigger);
        }
    }

    void OnDestroy() {
        if (immediateEffect) {
            EventBus<TriggerWasSet>.Unsub(OnTrigger);
        }
    }

    private void TryToReadTrigger() {
        if (trigger != null && trigger != "")
            if (Player.Instance.GetTrigger(trigger))
            foreach (var target in targets)
            {
                target.SetActive(setActive);
            }
    }

    private void OnTrigger(TriggerWasSet message)
    {
        TryToReadTrigger();
    }
}
