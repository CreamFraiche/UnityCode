using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNinja : NPC
{
    private Vector3 spellPos;
    public void Awake()
    {
        spellPos = projectileWeapon.transform.position;
    }

    new public void HitStart()
    {
        if (Random.Range(0, 10) > 5 || weapon == null)
        {
            Debug.Log("Mage Hit Start");
            projectileWeapon.transform.position = transform.position + transform.forward * 2;
            projectileWeapon.GetComponent<Projectile>().SetTarget(player);
            projectileWeapon.SetActive(true);
        }
        else
        {
            base.HitStart();
        }

    }
}
