using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{

    public void Start()
    {

    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PlayerWeapon" || other.gameObject.tag == "EnemyWeapon")
        {
            gameObject.SetActive(false);
        }
    }
}
