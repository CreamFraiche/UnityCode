using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] comboIcons;

    public void SetIcon(int nextAttack)
    {
        int i = 0;
        foreach(GameObject icon in comboIcons)
        {
            if (i == nextAttack)
            {
                icon.GetComponent<Image>().color = new Color32(255, 255, 0, 100);
            }
            else
            {
                icon.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            }
            i++;
        }
    }
}
