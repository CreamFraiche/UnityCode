using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{   
    public GameObject[] attackUI;

    //Does storing references work here? or do we have to directly talk to player
    public List<Attack> comboList;
    public List<Attack> attackInv;

    public GameObject itemUIPrefab;
    public GameObject contentBox;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttacks(List<Attack> attacks)
    {
        comboList = attacks;
        Debug.Log("combo length: " + attacks.Count);
        int i = 0;
        foreach(Attack curAtt in attacks) {
            attackUI[i].GetComponent<AttackInfoUI>().SetAttack(curAtt);
            attackUI[i].GetComponent<AttackInfoUI>().ToggleStats(true);
            i++;
        }
        while(i < 5)
        {
            attackUI[i].GetComponent<AttackInfoUI>().NameLocked();
            i++;
        }
    }

    public void ShowAttackList(List<Attack> invAttacks)
    {

        int childs = contentBox.transform.childCount;
        for (int t = childs - 1; t >= 0; t--)
        {
            Destroy(contentBox.transform.GetChild(t).gameObject);
        }

        attackInv = invAttacks;
        int i = 0;
        foreach (Attack att in invAttacks)
        {
            Debug.Log(att.title);
            GameObject newObj = Instantiate(itemUIPrefab, new Vector3(282, 24 + (-48 * i), 0), Quaternion.identity);
            newObj.transform.SetParent(contentBox.transform);

            RectTransform rt = newObj.GetComponent<RectTransform>();
            //rt.SetPositionAndRotation(new Vector3(282, 24 + (-48 * i), 0), Quaternion.identity);
            rt.offsetMax = new Vector2Int(0, -60 + (-48 * i));
            rt.offsetMin = new Vector2Int(0, -60 + (-48 * i));
            rt.sizeDelta = new Vector2(0, 50);



            //rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 780);
            //rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 10, 50);
            //rt.position.Set(rt.position.x, rt.position.x + (15 * i), rt.position.z);

            //Check out https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/HOWTO-UICreateFromScripting.html#:~:text=In%20order%20to%20be%20able,make%20it%20into%20a%20prefab.
            //Set Attack
            newObj.GetComponent<AttackInfoUI>().SetAttack(att);
            newObj.GetComponent<AttackInfoUI>().curIndex = i;
            i++;
        }
    }

    public void EquipAttack(Attack newAtt, int comboIndex)
    {

        attackInv.Remove(newAtt);

        if(comboList.Count > comboIndex)
        {
            attackInv.Add(comboList[comboIndex]);
            comboList[comboIndex] = newAtt;
        }
        else
        {
            //This will not work if they do #4 before 3
            comboList.Add(newAtt);
        }
        ShowAttackList(attackInv);
        HUDController.SetMouseImage(false);
    }
}
