using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMage : NPC
{
    private Vector3 spellPos;
    public void Awake()
    {
        spellPos = projectileWeapon.transform.position;
    }
    new public void HitStart()
    {
        Debug.Log("Mage Hit Start");
        projectileWeapon.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward * 2;
        projectileWeapon.GetComponent<Projectile>().SetTarget(player);
        projectileWeapon.SetActive(true);
    }

    new public void HitEnd()
    {
        //Maybe another animation
    }
}
