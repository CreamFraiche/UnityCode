using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    public List<Item> Inventory;
    public List<Equipment> equipment;
    public EquipmentInventory equipmentInventory;
    public int gold, food, health, maxHealth;
    public GameObject sword;

    public PlayerController playerController;

    //Sounds
    public AudioClip[] sounds;
    public AudioClip stepSound, swordSound, coinSound, pickupSound, blockSound;
    private AudioSource audioSrc;

    public void Start()
    {
        //Debug Quest "Int Setter"
        //PlayerPrefs.SetInt("Bandits1", 0);
        audioSrc = gameObject.GetComponent<AudioSource>();
        Inventory = new List<Item>();
        gold = PlayerPrefs.GetInt("Gold");
        food = PlayerPrefs.GetInt("Food");
    }

    public void Awake()
    {
        gold = PlayerPrefs.GetInt("Gold");
        food = PlayerPrefs.GetInt("Food");
        Debug.Log("Loading Gold: " + gold);
        HUDController.Setup();
        HUDController.AddFood(food, 0);
        HUDController.AddGold(gold, 0);
        maxHealth = 100 + maxHealth;
        health = maxHealth;
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Gold")
        {
            ChangeGold(other.gameObject.GetComponent<HasValue>().GetValue());
            other.gameObject.GetComponent<HasValue>().Collected();
        }
        if (other.gameObject.tag == "Food")
        {
            CollectFood(other.gameObject.GetComponent<HasValue>().GetValue());
            other.gameObject.GetComponent<HasValue>().Collected();
        }

        if (other.gameObject.tag == "Potion")
        {
            ReceiveDamage(other.gameObject.GetComponent<Potion>().GetValue() * -1);

            other.gameObject.GetComponent<Potion>().Collected();
        }

        //Collectables are: Equipment, Attacks, Quest Items, anything that goes to inventory
        //Maybe attacks should be its own thing
        if (other.gameObject.tag == "Collectable")
        {
            PlaySound(pickupSound);
            Item newItem = other.gameObject.GetComponent<Collectible>().Collect();


            //Add item

        }

        if (other.gameObject.tag == "Attack")
        {
            Item newItem = other.gameObject.GetComponent<Collectible>().Collect();
            playerController.attackInv.Add(newItem.attack);
        }

        if (other.gameObject.tag == "EnemyWeapon")
        {
            Debug.Log("Player hit by enemy");
            if(playerController.status != CharacterStatus.Block)
            {
                ReceiveDamage(10);
            }
            else
            {
                PlaySound(blockSound);
            }
            if (other.gameObject.GetComponent<MeshCollider>())
            {
                other.gameObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    public void SetController(PlayerController playCtrl)
    {
        playerController = playCtrl;
        ApplyEquipment();
    }

    public void ReceiveDamage(int amt)
    {
        HUDController.ChangeHealth(health, 100);
        health -= amt;
        playerController.DamageAnimate();
        PlaySound(sounds[Random.Range(0, sounds.Length)]);
        if(health <= 0)
        {
            //Should not happen right away
            SceneManager.LoadScene("City1");
        }
    }

    public void ChangeGold(int amt)
    {
        gold += amt;
        PlaySound(coinSound);
        HUDController.AddGold(amt, gold);
    }

    public void ReceiveItem(Item item)
    {
        Debug.Log(item);
        if (item.itemName == "Food")
        {
            CollectFood(item.amount);
        }
        else if (item.itemName == "Small Potion")
        {
            Inventory.Add(item);
        }
        else if (item.attack != null)
        {
            playerController.attackInv.Add(item.attack);
        }
    }

    public void ApplyEquipment()
    {
        playerController.attackSpeed = 0;
        playerController.walkingSpeed = 0;
        playerController.jumpSpeed = 0;
        playerController.runningSpeed = 0;
        playerController.power = 0;
        maxHealth = 0;

        foreach (Equipment curEquip in equipment)
        {
            playerController.SetStats(curEquip);
            maxHealth += maxHealth;
            //sword will store power (i guess)
            //add any gameObjects
            if(curEquip.type == EquipmentType.Weapon)
            {
                GameObject newSword = Instantiate(curEquip.itemOBJ);
                newSword.transform.parent = sword.transform.parent;
                newSword.transform.position = sword.transform.position;
                newSword.transform.rotation = sword.transform.rotation;
                sword.SetActive(false);
                sword = newSword;
            }
        }
    }


    public void CollectFood(int amt)
    {
        PlaySound(pickupSound);
        int newFood = amt;
        food += newFood;
        HUDController.AddFood(newFood, food);
    }

    public void HitStart()
    {
        Debug.Log("HitStart Player");
        sword.GetComponent<MeshCollider>().enabled = true;
        PlaySound(swordSound);
    }

    public void HitEnd()
    {
        sword.GetComponent<MeshCollider>().enabled = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSrc.clip = clip;
        audioSrc.Play();
    }

    public void SaveValues()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Food", food);
        PlayerPrefs.Save();
    }

    public void FootR()
    {
        PlaySound(stepSound);
    }

    public void FootL()
    {
        PlaySound(stepSound);
    }

    public void Hit()
    {
        //So it will shut up
    }
}
