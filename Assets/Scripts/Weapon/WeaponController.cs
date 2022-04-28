using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;

    [SerializeField]
    private Image press;

    private void OnTriggerEnter(Collider other)
    {
        ActivateUI();
        other.GetComponent<MovementController>().availableItem = gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        ActivateUI();
        other.GetComponent<MovementController>().availableItem = null;
    }

    public void ActivateUI()
    {
        press.gameObject.SetActive(!press.gameObject.activeSelf);
    }
}
