using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public ItemToolTip_UI itemToolTip;
    public StatToolTip_UI statToolTip;

    private void Awake()
    {
        //if(instance != null)
        //{
        //    Destroy(instance);
        //}
        //else
        //{
        //    instance = this;
        //}

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
