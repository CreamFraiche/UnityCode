using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon" || collision.gameObject.tag == "EnemyWeapon")
        {
            Destroy(gameObject);
        }
    }
}
