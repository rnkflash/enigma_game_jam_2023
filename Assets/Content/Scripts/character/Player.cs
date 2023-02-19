using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public string toScene = null;
    public string toExit = null;
    public bool playerWasSprinting = false;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public Dictionary<string, bool> triggers = new Dictionary<string, bool>();

    public void AddItem(ItemData item, int amount) {
        if (!inventory.ContainsKey(item))
            inventory[item] = 0;
        inventory[item] += amount;
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
}
