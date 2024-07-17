using System.Collections;
using System.Data;
using UnityEngine;

public class Player : Entity
{
    public SkillManager skill { get; private set; }
    public GameObject sword {  get; private set; }

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public float wallJumpXSpeed;
    public float wallJumpDuration;

    [Header("Attack Info")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }

    public bool isBusy { get; private set; }

    #region States and Statemachine
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerThrowSwordState throwSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerReleaseBlackholeSkillState blackholeSkillState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "ThrowSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeSkillState = new PlayerReleaseBlackholeSkillState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.UseSkillIfAvailable())
        {
            //if current state is AimSwordState or ThrowSwordState, hide the aim dots first
            if (stateMachine.currentState == aimSwordState || stateMachine.currentState == throwSwordState)
            {
                skill.sword.ShowDots(false);
            }

            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        //Debug.Log("Is busy");
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
        //Debug.Log("not busy");
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    //if no sword, return null
    //if has sowrd, return the sword and return true;
    public bool HasNoSword()
    {
        if (!sword)
        {
            return true;
        }

        sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
    
}
