using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Singleton<Player>
{
    public bool dreamScenePassed = true;
    public string toScene = null;
    public string toExit = null;
    public bool playerWasSprinting = false;
    public bool cosmonaft = false;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public Dictionary<string, bool> triggers = new Dictionary<string, bool>();

    public void AddItem(ItemData item, int amount) {
        if (!inventory.ContainsKey(item))
            inventory[item] = 0;
        inventory[item] += amount;

        EventBus<PickedUpItem>.Pub(new PickedUpItem() {item = item, amount = amount});
    }

    public bool HasItem(ItemData item)
    {
        if (item == null)
            return false;
        return inventory.ContainsKey(item) && inventory[item] > 0;
    }

    public bool HasTrigger(string trigger) {
        return triggers.ContainsKey(trigger);
    }

    public bool GetTrigger(string trigger) {
        return triggers.ContainsKey(trigger) ? triggers[trigger] : false;
    }

    public void SetTrigger(string trigger, bool value) {
        triggers[trigger] = value;
        EventBus<TriggerWasSet>.Pub(new TriggerWasSet() { trigger = trigger, value = value});
    }

    internal bool HasItemOfType(ItemId pistol)
    {
        return inventory.Any(i=>i.Key.id == pistol);
    }
}
