using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int value;

    public int GetValue()
    {
        return value;
    }

    public void Collected()
    {
        gameObject.SetActive(false);
    }
}
