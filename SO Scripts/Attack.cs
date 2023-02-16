using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game Design/Attack")]
public class Attack : ScriptableObject
{
    public string title;
    public int dmg, curKills, animationStyle, weaponStyle, dashTime, dashSpeedX, dashSpeedY;
    public float speed, recoveryTime, forceBack, forceUp, fxDelay;
    public GameObject fx;
    public Sprite icon;
    public AudioClip audio;
}
