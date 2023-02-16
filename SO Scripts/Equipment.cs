using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Design/Equipment")]
public class Equipment : Item
{

    public EquipmentType type;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpSpeed;
    public float attackSpeed;
    public int armor;
    public int power;
    public int maxHealth;
}
