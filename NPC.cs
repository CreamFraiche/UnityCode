using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;
    public int actionChance, curHP, totalHP, curCombo;
    public string actions;
    public bool turning, isEnemy, isDead, debug;
    public int gold = 0;

    public GameObject curTarget, player, walkTarget;

    public Vector3 targetAngle;

    public GameObject cubeHP, weapon, projectileWeapon;
    public GameObject coin;

    public CharacterStatus status;

    public List<Attack> comboList;

    private Coroutine action, turn;

    //Game Design Vars
    public float attackDist;
    public int sightAngle;
    public int walkStyle = 0;

    //Sounds
    public AudioClip[] sounds;
    private AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        player = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        audioSrc = gameObject.GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        //Ragdoll Setup
        Rigidbody[] childRGs = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childRGs)
        {
            rb.isKinematic = true;
        }
        rigidbody.isKinematic = false;


        if (actions == "walk")
        {
            animator.SetBool("isWalking", true);
            animator.SetInteger("WalkStyle", walkStyle);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        totalHP = curHP;
        animator.SetFloat("AnimationSpeed", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (turning)
            {
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngle, 2f * Time.deltaTime);
                StartCoroutine(stopTurn());
            }

            if (curTarget)
            {
                float targDistance = Vector3.Distance(curTarget.transform.position, transform.position);
                if (targDistance < attackDist)
                {
                    if (comboList.Count > 0)
                    {
                        Debug.Log("Looking for combo");
                        curCombo++;
                        if (curCombo >= comboList.Count)
                        {
                            curCombo = 0;
                        }
                        Attack(comboList[curCombo]);
                        action = null;
                    }
                    else
                    {
                        animator.SetTrigger("attack1");
                    }
                }
                else if(targDistance < 5f && Input.GetMouseButtonDown(0))
                {
                    if(Random.Range(0,100) < 20) //20 is dodge chance
                    {
                        rigidbody.AddForce(-transform.forward * 10000f * Time.deltaTime);
                        animator.SetTrigger("recoil1");
                        Debug.Log("NPC Dodge");
                    }
                }
                else
                {

                    Debug.Log("Setting new action");
                    //Need to add projectile attack here
                    animator.SetBool("isWalking", true);
                }

                transform.LookAt(new Vector3(curTarget.transform.position.x, transform.position.y, curTarget.transform.position.z));
            }
            else
            {
                int random = Random.Range(1, 100);

                if (random <= actionChance && curTarget == null)
                {
                    if (actions == "walk")
                    {
                        animator.SetBool("isWalking", false);
                        action = StartCoroutine(StartWalking());
                    }
                    else if (actions == "attack")
                    {
                        animator.SetTrigger("attack1");
                    }
                }
                if (isEnemy)
                {
                    if (DetectPlayer())
                    {
                        //LineCast might need some work to work properly
                        //There should also be a distance check
                        if (!Physics.Linecast(transform.position, player.transform.position))
                        {
                            Debug.Log("LineCast unobstructed");
                            animator.enabled = true;
                            curTarget = player;
                        }
                    }
                }
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //Debug.Log("From Solder: " + other.gameObject);
        if(other.gameObject.tag == "PlayerWeapon" && isEnemy && !isDead)
        {
            Debug.Log("Enemy Hit");
            curTarget = other.gameObject;
            Weapon playerWeapon = other.gameObject.GetComponent<Weapon>();

            animator.enabled = true;
            //animator.applyRootMotion = false;
            if (playerWeapon)
            {
                rigidbody.AddForce(transform.up * playerWeapon.attackInfo.forceUp * 1000f * Time.deltaTime);
                rigidbody.AddForce(-transform.forward * playerWeapon.attackInfo.forceBack * 1000f * Time.deltaTime);
                TakeDamage(playerWeapon.finalPower);
            }
            TakeDamage(1);
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "NPC")
        {
            transform.Translate(Vector3.right * Time.deltaTime);
            if (Random.Range(0,4) == 3 && !turning)
            {
                Turn(360);
            }
        }
        else if(other.gameObject.tag == "NPC Collision")
        {
            action = StartCoroutine(StartWalking());
        }
    }

    IEnumerator StartWalking()
    {
        yield return new WaitForSeconds(2);
        turning = false;
        animator.applyRootMotion = true;
        animator.SetBool("isWalking", true);
        if (walkTarget)
        {
            SetNode(walkTarget);
        }
        else
        {
            GameObject foundNode = GetClosestObject(GameObject.FindGameObjectsWithTag("WalkingNode"));
            if (foundNode)
            {
                SetNode(foundNode);
            }
        }
        if(action != null)
        {
            StopCoroutine(action);
        }
    }


    IEnumerator stopTurn()
    {
        yield return new WaitForSeconds(2.5f);
        turning = false;
        //StartCoroutine(StartWalking());
    }

    public void Turn(int rotation = 180)
    {
        if (turning) { return;  }
        turning = true;
        targetAngle = new Vector3(0, rotation+gameObject.transform.rotation.y, 0); // what the new angles should be
    }

    IEnumerator ChangeAction(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Waited for time: " + time);
        int randRoll = Random.Range(-2, 4);
        Debug.Log(randRoll);
        if(randRoll < 0)
        {
            BlockAnimate();
        }
        else if (randRoll > 0 && randRoll < 1)
        {
            animator.SetBool("isWalking", true);
            animator.SetTrigger("lStrafe");
        }
        else if (randRoll >= 1 && randRoll <= 2)
        {
            animator.SetBool("isWalking", true);
            animator.SetTrigger("rStrafe");
        }
        else if (randRoll > 2)
        {
            Debug.Log("NPC random attack");

            animator.SetTrigger("attack1");
        }
        action = null;

    }

    public void FootL()
    {

    }
    public void FootR()
    {

    }

    public void Hit()
    {

    }

    public void HitStart()
    {
        Debug.Log("Hit Start " + gameObject.name);
        weapon.GetComponent<MeshCollider>().enabled = true;
    }

    public void HitEnd()
    {
        weapon.GetComponent<MeshCollider>().enabled = false;
    }

    public void Attack(Attack attInfo)
    {
        Debug.Log("Enemy Attack Performing: " + attInfo.title);
        //sword.GetComponent<Weapon>().attackInfo = attInfo;
        animator.SetFloat("AnimationSpeed", attInfo.speed);
        //animator.SetInteger("Weapon", attInfo.weaponStyle);
        animator.SetInteger("Action", 1 + attInfo.animationStyle); //Create a cool way to do combos
        animator.SetBool("Moving", false);
        //canMove = false;
        //animator.SetTrigger("Trigger");
        //StartCoroutine(ResetMove(attInfo.recoveryTime));
        if (attInfo.fx != null)
        {
            Vector3 newLocation = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            GameObject clone = Instantiate(attInfo.fx, newLocation, transform.rotation);
            clone.tag = "EnemyWeapon"; 
        }
    }

    public void TakeDamage(int amt)
    {
        HitSound();
        curHP -= amt;

        float hpPercent = (float)totalHP / (float)curHP;

        if(curHP > 0)
        {
            animator.SetTrigger("recoil1");
            cubeHP.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            //Cube is wrong here
            var cubeRenderer = cubeHP.GetComponent<Renderer>();
            Color healthColor;
            if (hpPercent > .77)
            {
                healthColor = new Color((hpPercent), 1, 0, .5f);
            }
            else
            {
                healthColor = new Color(1, (hpPercent), 0, .5f);
            }

            cubeRenderer.material.SetColor("_Color", healthColor);
        }
        else
        {
            Die();
        }
    }

    private bool DetectPlayer()
    {
        if (debug) { Debug.Log(Vector3.Angle(transform.forward, player.transform.position - transform.position)); }
        return (Vector3.Angle(transform.forward, player.transform.position - transform.position) < sightAngle);
    }

    private void Die()
    {
        if (action != null) { StopCoroutine(action); }

        cubeHP.SetActive(false);
        gameObject.GetComponent<NPC>().enabled = false;
        animator.SetBool("die", true);

        //Ragdoll;
        animator.enabled = false;
        foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }

        isDead = true;
        curTarget = null;
        int i = 0;
        while(i < gold && coin)
        {
            Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            i++;
        }
    }

    public void SetNode(GameObject walkNode)
    {
        walkTarget = walkNode;
        gameObject.transform.LookAt(new Vector3(walkNode.transform.position.x, gameObject.transform.position.y, walkNode.transform.position.z));
    }

    public GameObject GetClosestObject(GameObject[] objects)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in objects)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    public void HitSound()
    {
        audioSrc.clip = sounds[Random.Range(0, 2)];
        audioSrc.Play();
    }

    public void BlockAnimate()
    {
        status = CharacterStatus.Block;
        animator.SetTrigger("block");
    }

    public void AnimatorOff()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
