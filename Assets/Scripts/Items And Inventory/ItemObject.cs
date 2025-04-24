using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickupItem()
    {
        InventorySystem.current.add(referenceItem);
        Destroy(gameObject);
        Debug.Log("Picked up item");
    }
}
