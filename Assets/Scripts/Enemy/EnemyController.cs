using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public StatsData enemyStats = new StatsData();

    private Animator enemyAnimator;

    [SerializeField]
    private GameObject weapon;
    private float range;
    private int damage;

    private Transform player;
    private bool isAttacking = false;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        //for testing
        enemyStats.health = 50;
        damage = 10;

        range = weapon.GetComponent<WeaponController>().weaponData.range;
        //damage = weapon.GetComponent<WeaponController>().weaponData.damage;
    }

    void Update()
    {
        if (enemyAnimator.GetBool("isSeeing"))
        {
            if (Vector3.Distance(transform.position, player.transform.position) > range)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.003f);
            }
            else if (!isAttacking)
            {
                Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CombatController>().enemy = gameObject;
            player = other.transform;
            enemyAnimator.SetBool("isSeeing", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CombatController>().enemy = null;
            player = null;
            enemyAnimator.SetBool("isSeeing", false);
        }
    }

    private void Attack()
    {
        var playerToHit = player;

        isAttacking = true;
        enemyAnimator.SetBool("isAttacking", true);

        StartCoroutine(Utility.TimedEvent(() =>
        {
            playerToHit.GetComponent<CombatController>().TakeDamage(damage);
        }, 1.5f));

        StartCoroutine(Utility.TimedEvent(() =>
        {
            isAttacking = false;
        }, 3f));
    }

    public void TakeDamage(int damage)
    {
        enemyStats.health -= damage;
        enemyAnimator.SetBool("isHit", true);

        if (enemyStats.health <= 0)
        {
            enemyAnimator.SetBool("isDead", true);
            player = null;

            Destroy(gameObject, 4f);
        }
    }
}
