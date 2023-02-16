using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Attack attackInfo;
    public int finalPower;
    public int weaponPower;

    public Attack GetAttackInfo()
    {
        return attackInfo;
    }

    public void SetAttackInfo(Attack cur, int dmg = 1)
    {
        finalPower = dmg + weaponPower;
        attackInfo = cur;
    }


}
