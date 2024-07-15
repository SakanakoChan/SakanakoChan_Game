using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CloneSkillController : MonoBehaviour 
{
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneDuration;
    private float cloneTimer;
    private float colorLosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;

    private bool cloneFacingRight = true;
    private float cloneFacingDirection = 1;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    public void SetupClone(float _cloneDuration, float _colorLosingSpeed, bool _canAttack)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }

        cloneDuration = _cloneDuration;
        colorLosingSpeed = _colorLosingSpeed;

        cloneTimer = cloneDuration;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                enemy.Damage(cloneFacingDirection);
            }
        }
    }

    private void FaceClosestTarget()
    {
        //find all the enemies in radius 25
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistanceToEnemy = Mathf.Infinity;
        
        //find closest enemy
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float currentDistanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (currentDistanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = currentDistanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        //if successfully found out the closest enemy
        if(closestEnemy != null)
        {
            //if clone is on the right side of the closest enmey, flip it
            //clone faces right by default
            if(transform.position.x > closestEnemy.position.x)
            {
                CloneFlip();
            }
        }

    }

    private void CloneFlip()
    {
        transform.Rotate(0, 180, 0);

        cloneFacingRight = !cloneFacingRight;
        cloneFacingDirection = -cloneFacingDirection;
    }

}
