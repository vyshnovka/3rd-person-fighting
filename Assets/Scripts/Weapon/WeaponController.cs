using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<MovementController>().availableItem = gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<MovementController>().availableItem = null;
    }
}
