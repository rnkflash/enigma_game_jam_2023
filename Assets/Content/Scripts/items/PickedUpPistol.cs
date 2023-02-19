using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpPistol : MonoBehaviour
{
    private PlayerController character;
    
    void Start() {
        EventBus<PickedUpItem>.Sub(OnPickUpItem);
        character = GetComponent<PlayerController>();
    }

    void OnDestroy() {
        EventBus<PickedUpItem>.Unsub(OnPickUpItem);
    }

    private void OnPickUpItem(PickedUpItem message)
    {
        if (character.pistoletoMode)
            return;

        if (message.item.id == ItemId.Pistol) {
            character.SetupCostume(character.cosmonaftMode, true);

        }
    }
}
