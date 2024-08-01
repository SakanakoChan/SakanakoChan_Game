using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftLIst_UI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform referenceCraftList;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> equipmentCraftList;
    [SerializeField] private List<CraftSlot_UI> craftSlotUIList;

    private void Start()
    {
        AssignCraftListUI();
    }

    private void AssignCraftListUI()
    {
        for (int i = 0; i < referenceCraftList.childCount; i++)
        {
            craftSlotUIList.Add(referenceCraftList.GetChild(i).GetComponent<CraftSlot_UI>());
        }
    }

    public void SetupCraftList()
    {
        //delete all the current craft slot UI
        for (int i = 0; i < craftSlotUIList.Count; i++)
        {
            Destroy(craftSlotUIList[i].gameObject);
        }

        craftSlotUIList = new List<CraftSlot_UI>();

        for (int i = 0; i < equipmentCraftList.Count; i++)
        {
            GameObject newCraftSlot = Instantiate(craftSlotPrefab, referenceCraftList);
            newCraftSlot.GetComponent<CraftSlot_UI>()?.SetupCraftSlot(equipmentCraftList[i]);
            craftSlotUIList.Add(newCraftSlot.GetComponent<CraftSlot_UI>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}
