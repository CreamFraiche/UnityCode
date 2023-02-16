using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public float walkingSpeed = 0;
    public float runningSpeed = 0;
    public float jumpSpeed = 0;
    public float attackSpeed = 0;
    public float dashSpeed = 2f;
    public int power = 0;

    [Header("Game Balancing")]
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float comboTime = 3f;
    public float _slopeAngle;

    [Header("Combo Attacks")]
    public List<Attack> comboList, attackInv;

    [Header("Debug")]
    public int curCombo;
    public CharacterStatus status;

    Animator animator;
    public GameObject playerCharacter, sword, light;
    public Coroutine comboFunction;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true, jumping = false, paused = false, isGrounded = false, dashing = false;

    private int dashSpeedX, dashSpeedY;

    void Awake()
    {
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCharacter.GetComponent<PlayerInteractions>().SetController(this);
        sword = playerCharacter.GetComponent<PlayerInteractions>().sword;
        animator = playerCharacter.GetComponent<Animator>();

        StartCoroutine(LoadAttacks());

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //ANIMATION SPEED WILL BE PART OF ATTACK SPEED
        animator.SetFloat("AnimationSpeed", .8f);

        animator.SetInteger("Weapon", 1);
        animator.SetInteger("Action", 1);
        animator.SetInteger("TriggerNumber", 4);
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX, curSpeedY;
        //Receive input and set speed if canMove
        if (dashing) {
            curSpeedX = dashSpeedX;
        }
        else
        {
            //Receive input and set speed if canMove
            curSpeedX = canMove ? (isRunning ? 8f + runningSpeed : 5f + walkingSpeed) * Input.GetAxis("Vertical") : 0;
        }

        curSpeedY = canMove ? (5f + walkingSpeed / 2) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        if(Mathf.Abs(curSpeedY) > 2f)
        {
            curSpeedX = curSpeedX / 2;
        }
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            JumpAnimate();
            moveDirection.y = jumpSpeed + 8f;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (dashing)
        {
            moveDirection.y = dashSpeedY * 1f;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= dashSpeed * Time.deltaTime;
        }
        else if (jumping)
        {
            jumping = false;
            LandAnimate();
        }
        else
        {
            //Attack Inputs
            if (Input.GetMouseButtonDown(0) && canMove)
            {
                //AttackAnimate(GetAttackInfo());
                AttackAnimate(comboList[curCombo]);
            }
            else if (Input.GetMouseButton(1))
            {
                BlockAnimate();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                canMove = true;
                animator.SetFloat("AnimationSpeed", attackSpeed +.8f);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !dashing)
            {
                dashSpeedX = (int)dashSpeed;
                dashing = true;
                animator.SetBool("Dashing", true);
                StartCoroutine(DashSet(.6f));
            }
            else
            {
                status = CharacterStatus.Idle;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                HUDController.ShowCombo(paused, comboList, attackInv);

                Pause();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HUDController.ShowMenu(paused);

                Pause();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                light.SetActive(!light.activeSelf);
            }
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (!dashing)
        {
            // Player and Camera rotation
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


        animator.SetFloat("Velocity Z", curSpeedX);
        animator.SetFloat("Velocity X", curSpeedY);


        //Animation Extras
        if ((curSpeedX != 0 || curSpeedY != 0) && canMove)
        {
            animator.SetFloat("AnimationSpeed", 1f);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    IEnumerator LoadAttacks()
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("load attacks count " + InventoryHolder.Instance.comboList.Count);
        if (InventoryHolder.Instance)
        {
            if (InventoryHolder.Instance.comboList.Count > 0)
            {
                comboList = InventoryHolder.Instance.comboList;
                attackInv = InventoryHolder.Instance.attackInv;
            }

        }
    }

    public void Pause()
    {
        //Pause logic
        paused = !paused;
        if (!paused) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    }

    public void SetStats(Equipment equip)
    {
        walkingSpeed += equip.walkingSpeed;
        jumpSpeed += equip.jumpSpeed;
        runningSpeed += equip.runningSpeed;
        attackSpeed += equip.attackSpeed;
        power += equip.power;
        StartCoroutine(SetAttackSpeed());
    }

    void GroundCheck()
    {
        //Unused, consider
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f + 0.1f))
        {
            _slopeAngle = (Vector3.Angle(hit.normal, transform.forward) - 90);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator SetAttackSpeed()
    {
        yield return new WaitForSeconds(.1f);
        animator.SetFloat("AnimationSpeed", attackSpeed + .8f);
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
        sword.GetComponent<Weapon>().SetAttackInfo(attInfo, attInfo.dmg + power);
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
            StartCoroutine(DashSet(attInfo.dashTime/10f));
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


}