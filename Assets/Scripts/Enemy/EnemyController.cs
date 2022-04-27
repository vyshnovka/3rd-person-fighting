using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnimator;
    private Transform player;

    [SerializeField]
    private GameObject weapon;

    private float attackOffset;

    void Start()
    {
        attackOffset = weapon.GetComponent<WeaponController>().weaponData.range;
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyAnimator.GetBool("isSeeing"))
        {
            transform.LookAt(player);

            transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.003f);

            if (Vector3.Distance(transform.position, player.transform.position) < attackOffset)
            {
                enemyAnimator.SetBool("isAttacking", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.transform;
        enemyAnimator.SetBool("isSeeing", true);
    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
        enemyAnimator.SetBool("isSeeing", false);
    }
}
