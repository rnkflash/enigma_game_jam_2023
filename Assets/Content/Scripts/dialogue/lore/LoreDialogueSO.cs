using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLore", menuName = "Static Data/DialogueLore")]
public class LoreDialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
	public string[] sentences;
}
