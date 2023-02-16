using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackInfoUI : MonoBehaviour
{
    public Text attackName, attackDmg, attackSpeed, upForce, backForce;
    public Image attackPicture;
    public GameObject statInfo;
    public Attack curAttack;

    public int curIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetSpeed(float val)
    {
        attackSpeed.text = val.ToString();
    }

    private void SetName(string name)
    {
        attackName.text = name;
    }

    private void SetDamage(int dmg)
    {
        attackDmg.text = dmg.ToString();
    }

    private void SetUpForce(float amt)
    {
        upForce.text = amt.ToString();
    }

    private void SetBackForce(float amt)
    {
        backForce.text = amt.ToString();
    }

    public void SetPicture(Sprite img)
    {
        attackPicture.sprite = img;
    }

    public void SetAttack(Attack attack)
    {
        Debug.Log("Setting Attack");
        curAttack = attack;
        SetName(attack.title);
        SetPicture(attack.icon);
        SetDamage(attack.dmg);
        SetSpeed(attack.speed);
        SetBackForce(attack.forceBack);
        SetUpForce(attack.forceUp);
        Debug.Log(attack);
    }

    public void ToggleStats(bool show)
    {
        statInfo.SetActive(show);
    }

    public void NameLocked()
    {
        SetName("Locked");
    }
}
