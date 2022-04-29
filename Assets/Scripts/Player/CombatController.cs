using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    public StatsData playerStats = new StatsData();

    private MovementController movementController;

    [NonSerialized]
    public GameObject enemy = null;

    [NonSerialized]
    public GameObject weaponInHands = null;

    [NonSerialized]
    public float range;
    [NonSerialized]
    public int damage;
    [NonSerialized]
    public int defaultDamage = 2;
    [NonSerialized]
    public float defaultRange = 1.5f;

    public Slider healthBar;

    void Start()
    {
        damage = defaultDamage;
        range = defaultDamage;

        healthBar.maxValue = playerStats.health;
        healthBar.value = playerStats.health;

        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<MovementController>().canMove)
        {
            Attack();
        }
        if (Input.GetMouseButton(1))
        {
            Block();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            movementController.canMove = true;
            movementController.characterAnimator.SetBool("isBlocking", false);
        }
    }

    private void Attack()
    {
        movementController.canMove = false;
        movementController.characterAnimator.SetBool("isAttacking", true);
        
        if (weaponInHands)
        {
            range = weaponInHands.GetComponent<WeaponController>().weaponData.range;
            damage = weaponInHands.GetComponent<WeaponController>().weaponData.damage;
        }

        if (weaponInHands && weaponInHands.GetComponent<ShootingSystem>())
        {
            weaponInHands.GetComponent<ShootingSystem>().Shoot();
        }
        else if (enemy)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                StartCoroutine(Utility.TimedEvent(() =>
                {
                    if (enemy)
                    {
                        enemy.GetComponent<EnemyController>().TakeDamage(damage);
                    }
                }, 1f));
            }
        }

        StartCoroutine(Utility.TimedEvent(() =>
        {
            GetComponent<MovementController>().canMove = true;
        }, 1.5f));
    }

    private void Block()
    {
        movementController.canMove = false;
        movementController.characterAnimator.SetBool("isBlocking", true);
    }

    public void TakeDamage(int damage)
    {
        if (!movementController.characterAnimator.GetBool("isBlocking"))
        {
            healthBar.value -= damage;
            playerStats.health -= damage;
            movementController.characterAnimator.SetBool("isHit", true);

            if (playerStats.health <= 0)
            {
                movementController.characterAnimator.SetBool("isDead", true);

                CanvasManager.instance.Wasted();

                GetComponent<CameraMovement>().enabled = false;
            }
        }
    }
}
