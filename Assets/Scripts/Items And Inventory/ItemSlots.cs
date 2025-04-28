using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    
    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_label;
    [SerializeField] private GameObject m_stackObject;
    [SerializeField] private TextMeshProUGUI m_stackLabel;

    public void Set(InventorySystem.InventoryItem item)
    {
        m_icon.sprite = item.data.Icon;
        m_label.text = item.data.Name;
        if (item.stackSize <= 1)
        {
            m_stackObject.SetActive(false);
            return;
        }
        m_stackLabel.text = item.stackSize.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
