using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [Space]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalExistenceDuration;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]  //spawn clone on original position when teleporting to crystal position
    [SerializeField] public bool spawnClone;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Crystal Gun")]
    [SerializeField] private bool canUseCrystalGun;
    [SerializeField] private int magSize;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shootWindow;
    private float shootWindowTimer;
    [SerializeField] private List<GameObject> crystalMag = new List<GameObject>();

    protected override void Update()
    {
        base.Update();

        shootWindowTimer -= Time.deltaTime;

        //if haven't shoot all the ammo in the mag for a while
        //auto reload the mag
        //shootWindowTimer = shootWindow; in ShootCrystalGunIfAvailable()
        if (shootWindowTimer < 0 && crystalMag.Count > 0)
        {
            ReloadCrystalMag();
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        //if in crystal gun mode, disabling all single crystal functions
        if (ShootCrystalGunIfAvailable())
        {
            return;
        }

        //if there's no crystal yet, create crystal
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            //if crystal can move towards enemy,
            //teleport function will be disabled
            if (canMove)
            {
                return;
            }

            //spawn clone then teleport
            //****************************************************************
            //***Cannot enable this when Replace Clone By Crystal is enable***
            //****************************************************************
            if (spawnClone)
            {
                SkillManager.instance.clone.CreateClone(player.transform.position);
                Destroy(currentCrystal);
            }

            //player teleport to the crystal's position
            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            currentCrystal.GetComponent<CrystalSkillController>()?.EndCrystal_ExplodeIfAvailable();
        }
    }

    public void CreateCrystal()
    {
        //create a crystal
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity); ;
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalExistenceDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    private void ReloadCrystalMag()
    {
        int ammoToAdd = magSize - crystalMag.Count;

        for (int i = 0; i < ammoToAdd; i++)
        {
            crystalMag.Add(crystalPrefab);
        }
    }

    private bool ShootCrystalGunIfAvailable()
    {
        if (canUseCrystalGun)
        {
            if(crystalMag.Count > 0)
            {
                GameObject crystalToSpawn = crystalMag[crystalMag.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity); ;

                crystalMag.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>()?.
                    SetupCrystal(crystalExistenceDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(newCrystal.transform));

                
                shootWindowTimer = shootWindow;


                if (crystalMag.Count <= 0)
                {
                    Invoke("ReloadCrystalMag", reloadTime);
                }
            }

            return true;
        }

        return  false;
    }

    public void DestroyCurrentCrystal()
    {
        if (currentCrystal != null)
        {
            Destroy(currentCrystal);
        }
    }

    public void CurrentCrystalSpecifyEnemy(Transform _enemy)
    {
        if (currentCrystal != null)
        {
            currentCrystal.GetComponent<CrystalSkillController>()?.SpecifyEnemyTarget(_enemy);
        }
    }

    //public Transform GetCurrentCrystalTransform()
    //{
    //    return currentCrystal?.transform;
    //}


    //public void CurrentCrystalChooseRandomEnemy(float _searchRadius)
    //{
    //    if (currentCrystal != null)
    //    {
    //        currentCrystal.GetComponent<CrystalSkillController>()?.CrystalChooseRandomEnemy(_searchRadius);
    //    }
    //}
}
