using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Skill Info")]
    [SerializeField] private SkillTreeSlot_UI throwSwordSkillUnlockButton;
    public bool throwSwordSkillUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchSpeed;
    [SerializeField] private float swordReturnSpeed;
    private float swordGravity;

    [Header("Regular Sword Info")]
    [SerializeField] private float regularSwordGravity;

    [Header("Bounce Sword Info")]
    [SerializeField] private SkillTreeSlot_UI bounceSwordUnlockButton;
    public bool bounceSwordUnlocked { get; private set; }
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceSwordGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Sword Info")]
    [SerializeField] private SkillTreeSlot_UI pierceSwordUnlockButton;
    public bool pierceSwordUnlocked { get; private set; }
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceSwordGravity;

    [Header("Spin Sword Info")]
    [SerializeField] private SkillTreeSlot_UI spinSwordUnlockButton;
    public bool spinSwordUnlocked { get; private set; }
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinHitCooldown;
    [SerializeField] private float spinSwordGravity;



    [Header("Passive Skill Info")]
    [SerializeField] private SkillTreeSlot_UI timeStopUnlockButton;
    [SerializeField] private float enemyFreezeDuration;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlot_UI vulnerabilityUnlockButton;
    [SerializeField] private float enemyVulnerableDuration;
    public bool vulnerabilityUnlocked { get; private set; }

    private Vector2 finalDirection;

    [Header("Aim Dots")]
    [SerializeField] private int dotNumber;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();

        GenerateDots();

        throwSwordSkillUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockThrowSwordSkill);
        bounceSwordUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockBounceSword);
        pierceSwordUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockPierceSword);
        spinSwordUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockTimeStop);
        vulnerabilityUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockVulnerability);
    }

    protected override void Update()
    {
        //calculate sword throw trajectory
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            finalDirection = new Vector2(AimDirection().normalized.x * launchSpeed.x, AimDirection().normalized.y * launchSpeed.y);
        }

        //adjust dots position according to player's aim position
        if (Input.GetKey(KeyCode.Mouse1))
        {
            SetupSwordGravity();
            //dots position will not get adjusted anymore when pressing mouse left
            if (player.stateMachine.currentState != player.throwSwordState)
            {
                for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = SetDotsPosition(i * spaceBetweenDots);
                }
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        SetupSwordGravity();

        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounceSword(true, bounceAmount, bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordScript.SetupPierceSword(true, pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpinSword(true, maxTravelDistance, spinDuration, spinHitCooldown);
        }

        newSwordScript.SetupSword(finalDirection, swordGravity, swordReturnSpeed, enemyFreezeDuration, enemyVulnerableDuration);

        player.AssignNewSword(newSword);

        ShowDots(false);
    }

    private void SetupSwordGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceSwordGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceSwordGravity;
        }
        else if (swordType == SwordType.Regular)
        {
            swordGravity = regularSwordGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinSwordGravity;
        }
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //get mouse cursor position

        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    private void GenerateDots()
    {
        dots = new GameObject[dotNumber];

        for (int i = 0; i < dotNumber; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void ShowDots(bool _showDots)
    {
        for (int i = 0; i < dotNumber; i++)
        {
            dots[i].SetActive(_showDots);
        }
    }

    private Vector2 SetDotsPosition(float t)
    {
        //б���˶�����������
        //ˮƽ��������ֱ���˶�, d = vt
        //��ֱ���������˶���h = v0t - 0.5gt^2  (v0ָ����)
        //����Physics2D.gravity�����ǳ��µ���������+��
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchSpeed.x,
            AimDirection().normalized.y * launchSpeed.y) * t  //����Ϊˮƽ�����d = vt ����ֱ����� v0t
            + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);  //�˴�����ֱ�������� -0.5gt^2 (����������������������+��)

        return position;
    }
    #endregion

    #region Unlock Skill
    private void UnlockThrowSwordSkill()
    {
        if (throwSwordSkillUnlocked)
        {
            return;
        }

        if (throwSwordSkillUnlockButton.unlocked)
        {
            throwSwordSkillUnlocked = true;
            //sword type is regular by default
            swordType = SwordType.Regular;
        }
    }

    private void UnlockBounceSword()
    {
        if (bounceSwordUnlocked)
        {
            return;
        }

        if (bounceSwordUnlockButton.unlocked)
        {
            bounceSwordUnlocked = true;
            swordType = SwordType.Bounce;
        }
    }

    private void UnlockPierceSword()
    {
        if (pierceSwordUnlocked)
        {
            return;
        }

        if (pierceSwordUnlockButton.unlocked)
        {
            pierceSwordUnlocked = true;
            swordType = SwordType.Pierce;
        }
    }

    private void UnlockSpinSword()
    {
        if (spinSwordUnlocked)
        {
            return;
        }

        if (spinSwordUnlockButton.unlocked)
        {
            spinSwordUnlocked = true;
            swordType = SwordType.Spin;
        }
    }

    private void UnlockTimeStop()
    {
        if (timeStopUnlocked)
        {
            return;
        }

        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    private void UnlockVulnerability()
    {
        if (vulnerabilityUnlocked)
        {
            return;
        }

        if (vulnerabilityUnlockButton.unlocked)
        {
            vulnerabilityUnlocked = true;
        }
    }

    #endregion
}
