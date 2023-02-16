using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUIController : MonoBehaviour
{

    public GameObject weaponPanel, armorPanel, accPanel, backpackPanel;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPanel(int typeInt)
    {
        EquipmentType type = (EquipmentType)typeInt;
        if(type == EquipmentType.Weapon)
        {
            weaponPanel.SetActive(true);
            armorPanel.SetActive(false);
            backpackPanel.SetActive(false);
            accPanel.SetActive(false);

        }
        else if (type == EquipmentType.Armor)
        {
            weaponPanel.SetActive(false);
            armorPanel.SetActive(true);
            backpackPanel.SetActive(false);
            accPanel.SetActive(false);

        }
        else if (type == EquipmentType.Backpack)
        {
            weaponPanel.SetActive(false);
            armorPanel.SetActive(false);
            backpackPanel.SetActive(true);
            accPanel.SetActive(false);
        }
        else if (type == EquipmentType.Accessory)
        {
            weaponPanel.SetActive(false);
            armorPanel.SetActive(false);
            backpackPanel.SetActive(false);
            accPanel.SetActive(true);
        }
    }

    public void TestMethod()
    {

    }
}
