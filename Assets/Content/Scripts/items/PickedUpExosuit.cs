using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpExosuit : MonoBehaviour
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

        if (message.item.id == ItemId.Exosuit) {
            character.SetupCostume(true, character.pistoletoMode);
            Player.Instance.cosmonaft = true;
            SoundSystem.PlaySound(Sounds.Instance.GetAudioClip(SoundId.exosuit_intro));
        }
    }
}
