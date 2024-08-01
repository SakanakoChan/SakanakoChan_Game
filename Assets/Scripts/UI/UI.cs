using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class UI : MonoBehaviour
{
    //public static UI instance;

    [SerializeField] private GameObject character_UI;
    [SerializeField] private GameObject skillTree_UI;
    [SerializeField] private GameObject craft_UI;
    [SerializeField] private GameObject options_UI;

    public ItemToolTip_UI itemToolTip;
    public StatToolTip_UI statToolTip;

    private GameObject currentUI;

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

    private void Start()
    {
        //No menu is open in the beginning of game
        SwitchToMenu(null);
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        //C for Character UI
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenMenuByKeyBoard(character_UI);
        }

        //B for Craft UI
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenMenuByKeyBoard(craft_UI);
        }

        //K for Skill Tree UI
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenMenuByKeyBoard(skillTree_UI);
        }

        //If there's already a UI open currently
        //Esc will close the UI
        //else, Esc will open Options UI
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentUI != null)
            {
                SwitchToMenu(null);
            }
            else
            {
                OpenMenuByKeyBoard(options_UI);
            }
        }
    }

    public void SwitchToMenu(GameObject _menu)
    {
        //close all the UIs
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            currentUI = null;
        }

        //set the target UI active
        if (_menu != null)
        {
            _menu.SetActive(true);
            currentUI = _menu;
        }

    }

    public void OpenMenuByKeyBoard(GameObject _menu)
    {
        //same as if(_menu != null && _menu.active == true)
        //activeSelf returns the active state of gameobject
        //Here means if re-entering (double press the same UI key) the same menu which is already open, then close the menu UI
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            currentUI = null;
        }
        else if (_menu != null && !_menu.activeSelf)  //if the menu to switch is not open, then switch to that menu
        {
            SwitchToMenu(_menu);
        }
    }
}
