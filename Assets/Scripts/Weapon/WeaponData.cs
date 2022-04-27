using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Scepter,
    Bow
}

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponType type;
    public float range;
    public int damage;

    public AnimatorOverrideController weaponAnimator;
    public Sprite icon;
}
