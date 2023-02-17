using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreDialogController: MonoBehaviour {

    public TMP_Text dialogueText;
    private Queue<string> sentences = new Queue<string>();
    public float timePerCharacter = 0.05f;
    private bool typingSentence = false;
    private string currentSentence;

    void Start() {
        dialogueText.text = "";
        sentences.Clear();
        EventBus<LoreDialogStart>.Sub(ShowLore);
        EventBus<LoreDialogNext>.Sub(SkipSentence);
        EventBus<LoreDialogEnd>.Sub(EndDialogue);
    }

    void OnDestroy() {
        EventBus<LoreDialogStart>.Unsub(ShowLore);
        EventBus<LoreDialogNext>.Unsub(SkipSentence);
        EventBus<LoreDialogEnd>.Unsub(EndDialogue);
    }

    public void ShowLore(LoreDialogStart msg) {
        typingSentence = false;
        dialogueText.text = "";
        sentences.Clear();
        foreach (string item in msg.payload.sentences)
        {
            sentences.Enqueue(item);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		currentSentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence());
	}

    private void SkipSentence(LoreDialogNext msg) {
        if (typingSentence)
        {
            StopAllCoroutines();
            typingSentence = false;

            dialogueText.text = currentSentence;
        } else
            DisplayNextSentence();
    }

	IEnumerator TypeSentence ()
	{
        typingSentence = true;
		dialogueText.text = "";
		foreach (char letter in currentSentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(timePerCharacter);
		}
        typingSentence = false;
	}

	void EndDialogue(LoreDialogEnd msg = null)
	{
        sentences.Clear();
        dialogueText.text = "";
        currentSentence = "";
        typingSentence = false;
        if (msg == null)
		    EventBus<LoreDialogEnd>.Pub(new LoreDialogEnd());
	}
}