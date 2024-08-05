using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //public static UI instance;

    [SerializeField] private GameObject character_UI;
    [SerializeField] private GameObject skillTree_UI;
    [SerializeField] private GameObject craft_UI;
    [SerializeField] private GameObject options_UI;
    [SerializeField] private GameObject ingame_UI;

    public SkillToolTip_UI skillToolTip;
    public ItemToolTip_UI itemToolTip;
    public StatToolTip_UI statToolTip;
    public CraftWindow_UI craftWindow;

    private GameObject currentUI;

    private void Awake()
    {
        //need this
        //or put UnlockSkill() in IPointerDownHandler in SkllTreeSlot_UI
        //to make sure the event listener order in skill tree ui is in correct order
        //skillTree_UI.SetActive(true);

        //if (instance != null)
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
        //No menu except ingame_UI is open in the beginning of game
        SwitchToMenu(ingame_UI);
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
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

        //If there's already a non-ingameUI open currently
        //Esc will close the UI and open ingame UI
        //else, Esc will open Options UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentUI != ingame_UI)
            {
                SwitchToMenu(ingame_UI);
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
        //Here means if re-entering (double press the same UI key) the same menu which is already open, then close the menu UI and open ingame UI
        if (_menu != null && _menu.activeSelf)
        {
            //_menu.SetActive(false);
            //currentUI = null;
            SwitchToMenu(ingame_UI);
        }
        else if (_menu != null && !_menu.activeSelf)  //if the menu to switch is not open, then switch to that menu
        {
            SwitchToMenu(_menu);
        }
    }

    public Vector2 SetupToolTipPositionOffsetAccordingToMousePosition(float _xOffsetRate_left, float _xOffsetRate_right, float _yOffsetRate_up, float _yOffsetRate_down)
    {
        Vector2 mousePosition = Input.mousePosition;
        float _xOffset = 0;
        float _yOffset = 0;

        //if mouse is on the right side of the screen
        if (mousePosition.x >= Screen.width * 0.5)
        {
            _xOffset = -Screen.width * _xOffsetRate_left;
        }
        else //if mouse is on the left side of the screen
        {
            _xOffset = Screen.width * _xOffsetRate_right;
        }

        //if mouse is on the upper side of the screen
        if (mousePosition.y >= Screen.height * 0.5)
        {
            _yOffset = -Screen.height * _yOffsetRate_down;
        }
        else //if mouse is on the lower side of the screen
        {
            _yOffset = Screen.height * _yOffsetRate_up;
        }

        Vector2 toolTipPositionOffset = new Vector2(_xOffset, _yOffset);
        return toolTipPositionOffset;
    }

    public Vector2 SetupToolTipPositionOffsetAccordingToUISlotPosition(Transform _slotUITransform, float _xOffsetRate_left, float _xOffsetRate_right, float _yOffsetRate_up, float _yOffsetRate_down)
    {
        float _xOffset = 0;
        float _yOffset = 0;

        //if slotUI is on the right side of the screen
        if (_slotUITransform.position.x >= Screen.width * 0.5)
        {
            _xOffset = -Screen.width * _xOffsetRate_left;
        }
        else //if slotUI is on the left side of the screen
        {
            _xOffset = Screen.width * _xOffsetRate_right;
        }

        //if slotUI is on the upper side of the screen
        if (_slotUITransform.position.y >= Screen.height * 0.5)
        {
            _yOffset = -Screen.height * _yOffsetRate_down;
        }
        else //if slotUI is on the lower side of the screen
        {
            _yOffset = Screen.height * _yOffsetRate_up;
        }

        Vector2 toolTipPositionOffset = new Vector2(_xOffset, _yOffset);
        return toolTipPositionOffset;
    }
}
