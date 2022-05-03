using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private List<GameObject> inventory = new List<GameObject>();
    public int inventorySize = 4;
    private int currentItemIndex = 0;

    [SerializeField]
    private GameObject visualInventory;

    private void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(null);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Equip(3);
        }
    }

    public bool AddToInventory(GameObject itemToAdd)
    {
        if (inventory.Count < inventorySize)
        {
            inventory.Add(itemToAdd);

            var child = visualInventory.transform.GetChild(inventory.IndexOf(itemToAdd));
            child.GetChild(0).GetComponent<Image>().enabled = true;
            child.GetChild(0).GetComponent<Image>().sprite = itemToAdd.GetComponent<WeaponController>().weaponData.icon;

            return true;
        }
        else
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = itemToAdd;

                    var child = visualInventory.transform.GetChild(i);
                    child.GetChild(0).GetComponent<Image>().enabled = true;
                    child.GetChild(0).GetComponent<Image>().sprite = itemToAdd.GetComponent<WeaponController>().weaponData.icon;

                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveFromInventory()
    {
        if (inventory[currentItemIndex])
        {
            var child = visualInventory.transform.GetChild(currentItemIndex);
            child.GetChild(0).GetComponent<Image>().sprite = null;
            child.GetChild(0).GetComponent<Image>().enabled = false;

            inventory[currentItemIndex].transform.parent = null;
            inventory[currentItemIndex].transform.position = new Vector3(inventory[currentItemIndex].transform.position.x, 0.1f, inventory[currentItemIndex].transform.position.z);
            inventory[currentItemIndex].GetComponent<Collider>().enabled = true;

            inventory[currentItemIndex] = null;
        }
    }

    public void Equip(int indexToEquip)
    {
        if (inventory[currentItemIndex])
        {
            inventory[currentItemIndex].SetActive(false);
        }

        if (inventory[indexToEquip])
        {
            inventory[indexToEquip].SetActive(true);
            GetComponent<MovementController>().characterAnimator.runtimeAnimatorController = inventory[indexToEquip].GetComponent<WeaponController>().weaponData.weaponAnimator;

            SetCurrentWeapon(inventory[indexToEquip]);
        }
        else
        {
            GetComponent<MovementController>().characterAnimator.runtimeAnimatorController = GetComponent<MovementController>().defaultAnimator;
            GetComponent<CombatController>().damage = GetComponent<CombatController>().defaultDamage;
            GetComponent<CombatController>().range = GetComponent<CombatController>().defaultRange;

            SetCurrentWeapon(null);
        }

        visualInventory.transform.GetChild(currentItemIndex).GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        currentItemIndex = indexToEquip;
        visualInventory.transform.GetChild(currentItemIndex).GetComponent<Image>().color = new Color32(255, 255, 255, 220);
    }

    public void FirstEquip(GameObject itemToEquip)
    {
        if (inventory[currentItemIndex] && inventory[currentItemIndex] != itemToEquip)
        {
            itemToEquip.SetActive(false);
        }
        else if (inventory.IndexOf(itemToEquip) != currentItemIndex)
        {
            itemToEquip.SetActive(false);
        }
        else
        {
            SetCurrentWeapon(itemToEquip);
            gameObject.GetComponent<MovementController>().characterAnimator.runtimeAnimatorController = itemToEquip.GetComponent<WeaponController>().weaponData.weaponAnimator;
        }
    }

    private void SetCurrentWeapon(GameObject currentWeapon)
    {
        GetComponent<CombatController>().weaponInHands = currentWeapon;
    }
}
