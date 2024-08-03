using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemStatInfo;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Transform referenceRequiredMaterialList;
    private Image[] materialImage;

    private void Awake()
    {
        materialImage = new Image[referenceRequiredMaterialList.childCount];

        for (int i = 0; i < referenceRequiredMaterialList.childCount; i++)
        {
            materialImage[i] = referenceRequiredMaterialList.GetChild(i).GetComponent<Image>();
        }
    }

    public void SetupCraftWindow(ItemData_Equipment _itemToCraft)
    {
        //clear the craft button events to prevent from redundant functions
        craftButton.onClick.RemoveAllListeners();

        if (_itemToCraft.requiredCraftMaterials.Count > materialImage.Length)
        {
            Debug.Log("Bug! Required craft materials are more than the material slots you made!");
        }

        //clear the color and the icon first
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        // an equipment needs 4 craft materials at most
        for (int i = 0; i < _itemToCraft.requiredCraftMaterials.Count; i++)
        {
            //setup the required materials (icon and needed amount)
            materialImage[i].sprite = _itemToCraft.requiredCraftMaterials[i].item.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI requiredMaterialText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            requiredMaterialText.text = _itemToCraft.requiredCraftMaterials[i].stackSize.ToString();
            requiredMaterialText.color = Color.white;
        }

        //setup the icon, name, stat info of the equipment to craft
        itemIcon.sprite = _itemToCraft.icon;
        itemName.text = _itemToCraft.itemName;
        itemStatInfo.text = _itemToCraft.GetItemStatInfoAndEffectDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CraftIfAvailable(_itemToCraft, _itemToCraft.requiredCraftMaterials));
    }
}
