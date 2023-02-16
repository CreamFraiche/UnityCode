using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDValueHealth : HUDValue
{
    public Image healthBar;

    public void ChangeHealth(int newHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)newHealth / (float)maxHealth;
    }
}
