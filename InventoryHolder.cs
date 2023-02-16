using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    public List<Attack> comboList, attackInv;
    public EquipmentInventory equipInventory;
    public static InventoryHolder Instance;
    public List<Attack> attackArchive;


    private void Start()
    {
        attackInv = LoadAttackInv();
        comboList = LoadCombo();
        Debug.Log(attackInv);
        Debug.Log(comboList);
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Save(List<Equipment> equipList)
    {
        int i = 0;
        foreach (Attack att in comboList)
        {
            Debug.Log("Saving ATT combo: " + att.name);
            PlayerPrefs.SetInt("comboList" + i, attackArchive.IndexOf(att));
            i++;
        }

        i = 0;
        foreach (Attack att in attackInv)
        {
            Debug.Log("Saving attackInv" + i + ": " + att.name + " as " + attackArchive.IndexOf(att));
            PlayerPrefs.SetInt("attackInv" + i, attackArchive.IndexOf(att));
            i++;
        }
        Debug.Log(equipList);
        Instance.SaveEquipment(equipList);
    }

    private void SaveEquipment(List<Equipment> equipList)
    {
        equipInventory.objInv = equipList;
    }

    public List<Attack> LoadCombo()
    {
        Instance.comboList = new List<Attack>();
        bool attackFound = true;
        int i = 0;

        while (attackFound)
        {
            int savedIndex = PlayerPrefs.GetInt("comboList" + i);
            if (savedIndex <= 0) { attackFound = false; break; }
            comboList.Add(attackArchive[savedIndex]);
            i++;
        }
        return comboList;
    }

    public List<Attack> LoadAttackInv()
    {
        Debug.Log("Load Inv");

        Instance.attackInv = new List<Attack>();
        bool attackFound = true;
        int i = 0;

        while (attackFound)
        {
            int savedIndex = PlayerPrefs.GetInt("attackInv" + i);
            Debug.Log("Player Pref: attackInv" + i);
            Debug.Log("equals : " + savedIndex);
            if (savedIndex <= 0) { attackFound = false; break; }
            attackInv.Add(attackArchive[savedIndex]);
            i++;
        }
        return attackInv;
    }

    public List<Equipment> LoadEquipmentInventory()
    {
        List<Equipment> curList = Instance.equipInventory.objInv;

        return curList;
    }
}