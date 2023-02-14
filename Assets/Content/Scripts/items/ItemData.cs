using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Static Data/Item")]
public class ItemData : ScriptableObject
{
  public ItemId id;
  public string itemName;
  public string description;
  public GameObject inventoryItemPrefab;
}
