using System;
using DG.Tweening;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
    private InteractiveObject interactiveObject;
    private DoorIndicator doorIndicator = null;
    public Collider doorCollider;
    public Animator animator = null;
    public ExitTrigger exit;
    public bool noAnimator = false;
    private bool sequenceOngoing = false;

    public bool locked = false;

	private void Start()
	{
		interactiveObject = GetComponentInChildren<InteractiveObject>();
        doorIndicator = GetComponentInChildren<DoorIndicator>();

        interactiveObject.onStartSelectListeners += OnSelect;
        interactiveObject.onStopSelectListeners += OnDeselect;
        interactiveObject.onInteractListeners += OnInteract;

	}

    void OnDestroy() {
        interactiveObject.onStartSelectListeners -= OnSelect;
        interactiveObject.onStopSelectListeners -= OnDeselect;
        interactiveObject.onInteractListeners -= OnInteract;
    }

    private void OnDeselect(PlayerInteraction interactor)
    {
        if (!sequenceOngoing)
            doorIndicator?.SetColor(DoorIndicator.Colors.YELLOW);
    }

    private void OnSelect(PlayerInteraction interactor)
    {
        if (!sequenceOngoing)
            doorIndicator?.SetColor(locked ? DoorIndicator.Colors.RED : DoorIndicator.Colors.GREEN);
    }

    private void OnInteract(PlayerInteraction interactor)
    {
        if (locked)
            return;
        
        ExitSequence(interactor.GetComponentInParent<PlayerController>());
    }

    public void EnterSequence(PlayerController player) {
        
        doorCollider.isTrigger = false;
        if (!noAnimator) {
            animator?.SetBool("IsOpen", false);
            animator?.Play("Base Layer.Open", 0, 1.0f);
        }
        exit.EnterSequence(player);
    }

    public void ExitSequence(PlayerController player) {
        sequenceOngoing = true;
        doorCollider.isTrigger = true;
        if (!noAnimator) {
            animator?.SetBool("IsOpen", true);
        }
        exit.ExitSequence(player);
    }
   
}