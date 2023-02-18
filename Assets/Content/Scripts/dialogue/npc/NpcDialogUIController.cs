using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NpcDialogUIController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public BlinkingTextButton[] choiceButtons;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SkipSentence() {

    }

    internal void StartDialog(NpcObject npc, TextAsset inky)
    {
        Debug.Log(inky);
    }
}
