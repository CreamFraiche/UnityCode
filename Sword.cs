using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int finalPower;
    public int curAttack;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerWeapon" || collision.gameObject.tag == "EnemyWeapon")
        {
            Debug.Log("Sword Clang");
            gameObject.GetComponent<MeshCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
