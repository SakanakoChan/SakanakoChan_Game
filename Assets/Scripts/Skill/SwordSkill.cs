using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Regular Sword Info")]
    [SerializeField] private float regularSwordGravity;

    [Header("Bounce Sword Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceSwordGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Sword Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceSwordGravity;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchSpeed;
    [SerializeField] private float swordReturnSpeed;
    private float swordGravity;

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

        newSwordScript.SetupSword(finalDirection, swordGravity, swordReturnSpeed);

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
        //斜抛运动，高中物理
        //水平方向匀速直线运动, d = vt
        //竖直方向上抛运动，h = v0t - 0.5gt^2  (v0指初速)
        //这里Physics2D.gravity本就是朝下的所以是用+号
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchSpeed.x,
            AimDirection().normalized.y * launchSpeed.y) * t  //到此为水平方向的d = vt 和竖直方向的 v0t
            + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);  //此处给竖直方向补上了 -0.5gt^2 (重力本就向下所以这里是+号)

        return position;
    }
    #endregion
}
