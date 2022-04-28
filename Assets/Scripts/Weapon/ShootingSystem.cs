using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    public void Shoot()
    {
        var currentBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z), Quaternion.Euler(0, transform.root.eulerAngles.y, transform.eulerAngles.z));

        Destroy(currentBullet, 2f);
    }
}
