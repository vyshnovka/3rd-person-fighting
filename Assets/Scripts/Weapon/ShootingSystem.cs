using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float timeToLive = 5f;

    public void Shoot()
    {
        var currentProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.45f, transform.position.z), Quaternion.Euler(0, transform.root.eulerAngles.y, transform.eulerAngles.z));

        Destroy(currentProjectile, timeToLive);
    }
}
