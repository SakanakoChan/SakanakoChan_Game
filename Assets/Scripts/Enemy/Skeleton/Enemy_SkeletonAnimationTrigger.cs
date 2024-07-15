using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Player player = hit.GetComponent<Player>();

                player.knockbackDirection = enemy.facingDirection;
                player.Damage(player.knockbackDirection);
            }
        }
    }

    private void OpenCounterAttackWindow()
    {
        enemy.OpenCounterAttackWindow();
    }

    private void CloseCounterAttackWindow()
    {
        enemy.CloseCounterAttackWindow();
    }
}
