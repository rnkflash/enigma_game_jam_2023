using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    private PlayerController player;
    private PlayerInteraction playerInteractions;

    void Start()
    {
        player = GetComponent<PlayerController>();
        playerInteractions = GetComponentInChildren<PlayerInteraction>();

        EventBus<ConfirmDialogStart>.Sub(DisablePlayer);
        EventBus<LoreDialogStart>.Sub(DisablePlayer);
        EventBus<CommentDialogStart>.Sub(DisablePlayer);
        EventBus<NpcDialogStart>.Sub(DisablePlayer);

        EventBus<ConfirmDialogResult>.Sub(EnablePlayer);
        EventBus<LoreDialogEnd>.Sub(EnablePlayer);
        EventBus<CommentDialogEnd>.Sub(EnablePlayer);
        EventBus<NpcDialogEnd>.Sub(EnablePlayer);
    }

    void OnDestroy()
    {
        EventBus<ConfirmDialogStart>.Unsub(DisablePlayer);
        EventBus<LoreDialogStart>.Unsub(DisablePlayer);
        EventBus<CommentDialogStart>.Unsub(DisablePlayer);
        EventBus<NpcDialogStart>.Unsub(DisablePlayer);

        EventBus<ConfirmDialogResult>.Unsub(EnablePlayer);
        EventBus<LoreDialogEnd>.Unsub(EnablePlayer);
        EventBus<CommentDialogEnd>.Unsub(EnablePlayer);
        EventBus<NpcDialogEnd>.Unsub(EnablePlayer);
    }

    private void DisablePlayer(Message anymsg = null)
    {
        playerInteractions.enabled = false;
        player.disableControls = true;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
    }

    private void EnablePlayer(Message anymsg = null)
    {
        playerInteractions.enabled = true;
        player.disableControls = false;
    }
}
