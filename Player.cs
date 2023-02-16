using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Gold;
    public GameObject curWeapon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            curWeapon.SetActive(!curWeapon.activeSelf);
        }
    }

    public void HideWeapon()
    {
        curWeapon.SetActive(false);
    }

    public void ShowWeapon()
    {
        curWeapon.SetActive(false);
    }
}
