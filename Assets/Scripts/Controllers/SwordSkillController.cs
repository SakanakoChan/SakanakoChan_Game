using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private float swordReturnSpeed;

    private float enemyFreezeDuration;

    [Header("Bounce Sword Info")]
    private bool isBouncingSword;
    private int bounceAmount;
    private float bounceSpeed;
    private List<Transform> bounceTargets = new List<Transform>();
    private int bounceTargetIndex;

    [Header("Pierce Sword Info")]
    private bool isPierceSword;
    private int pierceAmount;

    [Header("Saw Spin Sword Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinSword;

    private float spinHitCooldown;
    private float spinHitTimer;

    private bool spinTimerHasBeenSetToSpinDuration = false;
    private float spinDirection;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, swordReturnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1.5)
            {
                player.CatchSword(); //catch and destroy the sword
            }
        }

        BounceSwordLogic();

        SpinSwordLogic();

        DestroySwordIfTooFar(30);
    }


    private void StopAndSpin()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        if (!spinTimerHasBeenSetToSpinDuration)
        {
            spinTimer = spinDuration;
        }

        spinTimerHasBeenSetToSpinDuration = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sword will not hit enemy when returning back to player
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
        }

        //spin sword will stop and spin when hittin the first enemy
        //in order to make spin sword hit cooldown work correctly
        //this if statement is necessary
        if (isSpinSword)
        {
            StopAndSpin();
            return;
        }
        else
        {
            DamageAndFreezeEnemy(collision, enemyFreezeDuration);
        }


        SetupBounceSwordTargets(collision);

        SwordStuckInto(collision);
    }


    private void DamageAndFreezeEnemy(Collider2D collision, float _enemyFreezeDuration)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            //knock back enemy and do damage
            player.stats.DoDamge(enemy.GetComponent<CharacterStats>());

            //freeze enemy
            enemy.StartCoroutine("FreezeEnemyForTime", _enemyFreezeDuration);
        }

    }

    private void SwordStuckInto(Collider2D collision)
    {
        //pierce sword can pierce through enemy but won't pierce through wall or ground
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        //spin sword will stop and spin when hittin the first enemy
        //spin sword can't stuck into enemy
        if (isSpinSword && collision.GetComponent<Enemy>() != null)
        {
            StopAndSpin();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //if there's no enemies to bounce between, sword will stuck into gournd
        if (isBouncingSword && bounceTargets.Count > 0)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void BounceSwordLogic()
    {
        if (isBouncingSword && bounceTargets.Count > 0)
        {
            //Debug.Log("Sword is bouncing between enemies");
            transform.position = Vector2.MoveTowards(transform.position, bounceTargets[bounceTargetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, bounceTargets[bounceTargetIndex].position) < 0.15f)
            {
                DamageAndFreezeEnemy(bounceTargets[bounceTargetIndex].GetComponent<Collider2D>(), enemyFreezeDuration);
                bounceTargetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncingSword = false;
                    isReturning = true;
                }

                if (bounceTargetIndex >= bounceTargets.Count)
                {
                    bounceTargetIndex = 0;
                }
            }
        }
    }

    private void SetupBounceSwordTargets(Collider2D collision)
    {
        //if the sword has hit the enemy
        if (collision.GetComponent<Enemy>() != null)
        {
            //if the sword is bounceSword, add enemies to its bounceTargets list
            if (isBouncingSword && bounceTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        bounceTargets.Add(hit.transform);
                    }
                }
            }

            //sort the bounceTargets by distance to player (from small to big)
            bounceTargets.Sort(new SortByDistanceToPlayer_BounceSwordTargets());
        }
    }

    private void SpinSwordLogic()
    {
        if (isSpinSword)
        {
            //if sword has reached the maxTravelDistance, stop and spin
            if (Vector2.Distance(player.transform.position, transform.position) >= maxTravelDistance && !wasStopped)
            {
                StopAndSpin();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                spinHitTimer -= Time.deltaTime;

                //spin sword will slowly move to enemy if entering spin mode
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinSword = false;
                }

                if (spinHitTimer < 0)
                {
                    spinHitTimer = spinHitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            DamageAndFreezeEnemy(hit, enemyFreezeDuration);
                        }
                    }
                }
            }
        }
    }

    public void SetupSword(Vector2 _launchSpeed, float _swordGravity, float _swordReturnSpeed, float _enemyFreezeDuration)
    {
        rb.velocity = _launchSpeed;
        rb.gravityScale = _swordGravity;
        swordReturnSpeed = _swordReturnSpeed;
        enemyFreezeDuration = _enemyFreezeDuration;

        if (!isPierceSword)
        {
            anim.SetBool("Rotation", true);
        }

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

    }

    public void SetupBounceSword(bool _isBounceSword, int _bounceAmount, float _bounceSpeed)
    {
        isBouncingSword = _isBounceSword;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
    }

    public void SetupPierceSword(bool _isPierceSword, int _pierceAmount)
    {
        isPierceSword = _isPierceSword;
        pierceAmount = _pierceAmount;
    }

    public void SetupSpinSword(bool _isSpinSword, float _maxTravelDistance, float _spinDuration, float _spinHitCooldown)
    {
        isSpinSword = _isSpinSword;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        spinHitCooldown = _spinHitCooldown;
    }

    public void ReturnSword()
    {
        //can't return sword while bouncing between enemies
        if (bounceTargets.Count > 0)
        {
            return;
        }

        //get rid of the impact of gravity when returning the sword
        //in order to make the sword directly fly back to the player when returning
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void DestroySwordIfTooFar(float _maxDistance)
    {
        if (Vector2.Distance(player.transform.position, transform.position) >= _maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
