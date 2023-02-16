using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDValue : MonoBehaviour
{
    public Text total, addAmt;
    public int totalVal;

    public void AddValue(int amt)
    {
        totalVal += amt;
        if(amt > 0)
        {
            addAmt.text = "+ " + amt.ToString();
        }
        else
        {
            addAmt.text = amt.ToString();
        }
        total.text = totalVal.ToString();
        StartCoroutine(HideAdd());
    }

    IEnumerator HideAdd()
    {
        yield return new WaitForSeconds(2f);
        addAmt.text = "";
    }
}
