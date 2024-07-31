using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public ItemToolTip_UI itemToolTip;

    private void Awake()
    {
        //itemToolTip = GetComponentInChildren<ItemToolTip_UI>();
    }

    public void SwitchToMenu(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }
}
