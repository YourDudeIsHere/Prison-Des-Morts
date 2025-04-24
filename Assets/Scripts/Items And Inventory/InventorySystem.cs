using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;
    [Serializable]
    public class InventoryItem
    {
        public InventoryItemData data { get; private set; }
        public int stackSize { get; private set; }
        
        //Can use for item MetaData.
        public InventoryItem(InventoryItemData source)
        {
            data = source;
            AddToStack();
        }
        public void AddToStack()
        {
            stackSize++;
        }
        //Method to be called when for example, player uses an item
        public void RemoveFromStack()
        {
            if (stackSize > 0)
            {
                stackSize--;
            }
            else
            {
                Debug.LogWarning("Stack size is already zero.");
            }
        }
    }
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public void add(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            
            if (value.stackSize <= 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
        
    }
}
