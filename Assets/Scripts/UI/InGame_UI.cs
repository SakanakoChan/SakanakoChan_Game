using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    //*****************************
    //Knowing Issue:
    //When opening other UI
    //InGameUI's skill icon cooldown indicator will be paused
    //because InGameUI is set as InActive when other UI is open
    //*****************************


    public static InGame_UI instance;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image throwSwordImage;
    [SerializeField] private Image blackholeImage;

    //[SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentCurrency;

    private SkillManager skill;
    private Player player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHPUI;
        }

        skill = SkillManager.instance;
        player = PlayerManager.instance.player;
        UpdateHPUI();
    }

    private void Update()
    {
        currentCurrency.text = PlayerManager.instance.GetCurrentCurrency().ToString("#,#");

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.dashUnlocked)
        {
            SetSkillCooldownImage(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skill.parry.parryUnlocked)
        {
            SetSkillCooldownImage(parryImage);
        }


        //crystal cooldown image is set in CrystalSkillController 
        //if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        //{
        //    SetSkillCooldownImage(crystalImage);
        //}

        if (Input.GetKeyDown(KeyCode.Mouse1) && skill.sword.throwSwordSkillUnlocked)
        {
            SetSkillCooldownImage(throwSwordImage);
        }

        if (Input.GetKeyDown(KeyCode.R) && skill.blackholeSkill.blackholeUnlocked)
        {
            SetSkillCooldownImage(blackholeImage);
        }

        //There's no SetFlaskCooldownImage() function because FillFlaskCooldownImage is in a different logic
        //and it'll do the same stuff on different kinds of flasks
        //CDs for different flasks are calculated seperately

        FillSkillCooldownImage(dashImage, skill.dash.cooldown);
        FillSkillCooldownImage(parryImage, skill.parry.cooldown);
        FillSkillCooldownImage(crystalImage, skill.crystal.GetCrystalCooldown());
        FillSkillCooldownImage(throwSwordImage, skill.sword.cooldown);
        FillSkillCooldownImage(blackholeImage, skill.blackholeSkill.cooldown);
        FillFlaskCooldownImage();
    }

    private void UpdateHPUI()
    {
        //total_maxHP = maxHP + vitality * 5
        slider.maxValue = playerStats.getMaxHP();
        slider.value = playerStats.currentHP;
    }

    private void SetSkillCooldownImage(Image _skillImage)
    {
        //0 means skill is ready to use, default color
        if (_skillImage.fillAmount <= 0)
        {
            //1 means skill is in cooldown, darker color
            _skillImage.fillAmount = 1;
        }
    }

    public void SetCrystalCooldownImage()
    {
        SetSkillCooldownImage(crystalImage);
    }

    private void FillSkillCooldownImage(Image _skillImage, float _cooldown)
    {
        if (_skillImage == null)
        {
            return;
        }

        if (_skillImage.fillAmount > 0)
        {
            //the percentage increasement of the cooldown progression as time goes by
            //1 means in cooldown, 0 means ready to use
            _skillImage.fillAmount -= (1 / _cooldown) * Time.deltaTime;
        }
    }

    private void FillFlaskCooldownImage()
    {
        Image _flaskImage = Flask_UI.instance.flaskCooldownImage;

        if (_flaskImage == null)
        {
            return;
        }

        //each flask should be ready to use in the beginning
        ItemData_Equipment flask = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Flask);

        if (flask == null)
        {
            return;
        }

        if (!flask.itemUsed)
        {
            _flaskImage.fillAmount = 0;
            return;
        }

        if (_flaskImage.fillAmount >= 0)
        {
            float timer = Time.time - flask.itemLastUseTime;
            _flaskImage.fillAmount = 1 - ((1 / flask.itemCooldown) * timer);
        }
    }
}
