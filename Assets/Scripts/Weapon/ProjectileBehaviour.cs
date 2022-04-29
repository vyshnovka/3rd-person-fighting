using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon;

    private float range;
    private int damage;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;

        range = weapon.GetComponent<WeaponController>().weaponData.range;
        damage = weapon.GetComponent<WeaponController>().weaponData.damage;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * 0.2f;
        //transform.position -= transform.up * 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Vector3.Distance(transform.position, startingPosition) <= range)
            {
                other.GetComponent<EnemyController>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
