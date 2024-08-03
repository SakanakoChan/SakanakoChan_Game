using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTip_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemStatInfo;

    [Range(1, 72)]
    [SerializeField] private int originalItemNameFontSize;

    private void Start()
    {
        itemNameText.fontSize = originalItemNameFontSize;
    }

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemStatInfo.text = item.GetItemStatInfoAndEffectDescription();

        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize *= 0.8f;
        }
        else
        {
            itemNameText.fontSize = originalItemNameFontSize;
        }

        gameObject.SetActive(true);

        //Debug.Log(itemNameText.text);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = originalItemNameFontSize;
        gameObject.SetActive(false);
    }
}
