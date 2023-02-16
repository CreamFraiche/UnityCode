using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterActive : MonoBehaviour
{
    public float timeShown;
    public bool destroy = false;
    void OnEnable()
    {
        StartCoroutine(HideObject());
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(timeShown);
        if (!destroy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
