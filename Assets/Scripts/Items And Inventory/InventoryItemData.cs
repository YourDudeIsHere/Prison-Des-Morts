using UnityEngine;
[CreateAssetMenu(menuName = "Inventory ItemData")]
public class InventoryItemData : ScriptableObject
{
   public string Id;
   public string Name;
   public Sprite Icon;
   public GameObject Prefab;
}
