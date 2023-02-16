using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxDistance;
    public float speed;
    public bool aimed, destroy;
    public GameObject target;
    void Awake()
    {
        if (aimed)
        {
            if (!target)
            {
                GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
                foreach (GameObject curObj in npcs)
                {
                    float distance = Vector3.Distance(curObj.transform.position, transform.position);
                    if (distance <= maxDistance)
                    {
                        if (curObj.GetComponent<NPC>().isEnemy)
                        {
                            target = curObj;
                            break;
                        }
                    }
                }
            }
            transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z));
        }

    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1f);
        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(GameObject curTarget)
    {
        target = curTarget;
        transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "PlayerWeapon" && collision.gameObject.tag !="Player")
        {

            if (gameObject.tag == "PlayerWeapon")
            {
                Debug.Log(collision.gameObject + " + " + gameObject.tag);
            }

            StartCoroutine(TurnOff());
        }
    }
}
