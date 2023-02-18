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
    public CommentDialogueSO lockedDialog;
    public CommentDialogueSO unlockDialog;
    public ItemData unlockItem;

    private PlayerInteraction lastInteractor;

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

        lastInteractor = null;
    }

    public void SetLocked(bool value) {
        locked = value;
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
        lastInteractor = interactor;
        if (locked) {
            if (Player.Instance.HasItem(unlockItem)) {
                locked = false;
                doorIndicator?.SetColor(DoorIndicator.Colors.GREEN);
                EventBus<CommentDialogStart>.Pub(new CommentDialogStart() {payload = unlockDialog});
                EventBus<CommentDialogEnd>.Sub(WaitForDialogEnd);
            }
            else
                EventBus<CommentDialogStart>.Pub(new CommentDialogStart() {payload = lockedDialog});
            return;
        }
        
        ExitSequence(interactor.GetComponentInParent<PlayerController>());
    }

    private void WaitForDialogEnd(CommentDialogEnd msg) {
        EventBus<CommentDialogEnd>.Unsub(WaitForDialogEnd);
        ExitSequence(lastInteractor.GetComponentInParent<PlayerController>());
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