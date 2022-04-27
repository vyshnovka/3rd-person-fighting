using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    private StatsData playerStats = new StatsData();

    private MovementController movementController;

    [NonSerialized]
    public GameObject enemy = null;

    [NonSerialized]
    public float range;
    [NonSerialized]
    public int damage;

    [SerializeField]
    private Slider healthBar;

    void Start()
    {
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

        if (enemy)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }

        StartCoroutine(Utility.TimedEvent(() =>
        {
            GetComponent<MovementController>().canMove = true;
        }, 1.2f));
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

                Time.timeScale = 0;

                GetComponent<CameraMovement>().enabled = false;
            }
        }
    }
}
