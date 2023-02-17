using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueComment", menuName = "Static Data/DialogueComment")]
public class CommentDialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
	public string[] sentences;
}
