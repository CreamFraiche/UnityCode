using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game Design/New Item")]
public class Item : ScriptableObject
{
    public GameObject itemOBJ;
    public Sprite itemIMG;
    public int value;
    public int amount;
    public string itemName;
    public Attack attack;

}
