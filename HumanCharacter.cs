using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCharacter : MonoBehaviour
{
    public GameObject weapon, weaponAux;

    [Header("Character Stats")]
    public float walkingSpeed = 0;
    public float runningSpeed = 0;
    public float jumpSpeed = 0;
    public float attackSpeed = 0;
    public float dashSpeed = 2f;
    public float comboTime = 1f;
    public int power = 0;

    public bool canMove, jumping, dashing;
    public int curCombo;
    private int dashSpeedX, dashSpeedY;

    public CharacterStatus status;
    public Coroutine comboFunction;

    public List<Attack> comboList, attackInv;

    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();


        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //ANIMATION SPEED WILL BE PART OF ATTACK SPEED
        animator.SetFloat("AnimationSpeed", 1);

        animator.SetInteger("Weapon", 1); //Should be weapon.getcomponent.Type

        //For Old Animator
        animator.SetInteger("Action", 1);
        animator.SetInteger("TriggerNumber", 4);
    }

    public void SetStats(Equipment equip)
    {
        walkingSpeed += equip.walkingSpeed;
        jumpSpeed += equip.jumpSpeed;
        runningSpeed += equip.runningSpeed;
        attackSpeed += equip.attackSpeed;
        power += equip.power;
    }

    IEnumerator ResetMove(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetInteger("Weapon", 1);
        animator.SetInteger("Action", 1);
        animator.SetInteger("TriggerNumber", 4);
        animator.SetFloat("AnimationSpeed", 1f);
        canMove = true;
    }

    IEnumerator LandSet(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        jumping = true;
    }

    IEnumerator DashSet(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetBool("Dashing", false);
        dashing = false;
    }

    IEnumerator ComboTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        curCombo = 0;
        HUDController.SetComboHUD(0);
    }

    IEnumerator ShowFX(float seconds, GameObject prefab)
    {
        yield return new WaitForSeconds(seconds);
        Vector3 newLocation = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        GameObject clone = Instantiate(prefab, newLocation, transform.rotation);
    }

    public void DamageAnimate()
    {
        animator.SetInteger("Weapon", 1);
        animator.SetInteger("Action", 3);
        animator.SetInteger("TriggerNumber", 12);
        animator.SetTrigger("Trigger");
        StartCoroutine(ResetMove(.3f)); //STAT Recoil speed
    }

    public void JumpAnimate()
    {
        animator.SetInteger("Jumping", 1);
        animator.SetInteger("Action", 1);
        animator.SetInteger("TriggerNumber", 1);
        animator.SetTrigger("Trigger");
        StartCoroutine(LandSet(.01f)); //To avoid First Frame ground Collisions
    }

    public void LandAnimate()
    {
        animator.SetInteger("Action", 1);
        animator.SetInteger("Jumping", 0);
        animator.SetTrigger("Trigger");
        canMove = false;
        StartCoroutine(ResetMove(.3f)); //STAT This is Landing Speed
    }

    public void AttackAnimate(Attack attInfo)
    {
        Debug.Log("Performing: " + attInfo.title);
        weapon.GetComponent<Weapon>().SetAttackInfo(attInfo, attInfo.dmg + power);
        animator.SetFloat("AnimationSpeed", attInfo.speed);
        animator.SetInteger("TriggerNumber", 4);
        animator.SetInteger("Weapon", attInfo.weaponStyle);
        animator.SetInteger("Action", 1 + attInfo.animationStyle); //Create a cool way to do combos
        animator.SetBool("Moving", false);
        canMove = false;
        animator.SetTrigger("Trigger");
        StartCoroutine(ResetMove(attInfo.recoveryTime));
        if (attInfo.fx != null)
        {
            StartCoroutine(ShowFX(attInfo.fxDelay, attInfo.fx));
        }

        if (attInfo.dashTime != 0)
        {
            dashSpeedX = attInfo.dashSpeedX;
            dashSpeedY = attInfo.dashSpeedY;
            dashing = true;
            StartCoroutine(DashSet(attInfo.dashTime / 10f));
        }

        curCombo++;
        if (curCombo >= comboList.Count)
        {
            curCombo = 0;
        }

        if (comboFunction != null) { StopCoroutine(comboFunction); }

        comboFunction = StartCoroutine(ComboTimeout(comboTime));
        HUDController.SetComboHUD(curCombo);

        //When swords collide turn them off possibly
        //possibly automatically block when not doing anything
    }

    public void SwitchWeapons()
    {
        if (weapon.gameObject.activeSelf)
        {
            weaponAux.gameObject.SetActive(true);
            weapon.gameObject.SetActive(false);
            animator.SetInteger("Weapon", 2);
        }
        else
        {
            weapon.gameObject.SetActive(true);
            weaponAux.gameObject.SetActive(false);
            animator.SetInteger("Weapon", 1);
        }

    }

    public void BlockAnimate()
    {
        status = CharacterStatus.Block;
        animator.SetInteger("TriggerNumber", 4);
        animator.SetInteger("Action", 7);
        animator.SetBool("Moving", false);
        animator.SetFloat("AnimationSpeed", .2f);
        canMove = false;
        animator.SetTrigger("Trigger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
