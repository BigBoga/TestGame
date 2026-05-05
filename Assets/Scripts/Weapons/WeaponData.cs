using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] public string weaponType;
    [SerializeField] public string weaponName;

    [Space]

    [SerializeField] public float weaponDamage;
    [SerializeField] public float weaponCooldown;

    [Header("Animation")]
    [SerializeField] public int animtionIdLayer;

    [Header("Effects")]
    [SerializeField] public GameObject slashEffect;
    [SerializeField] public float timeBeforeSlash;
}
