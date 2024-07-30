using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        //Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Weapon)?.ReleaseSwordArcane();
        Inventory.instance.ReleaseSwordArcane_ConsiderCooldown();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamge(_target);

                //Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Weapon)?.ExecuteItemEffect(_target.transform);
                Inventory.instance.UseSwordEffect_ConsiderCooldown(_target.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
