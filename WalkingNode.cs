using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNode : MonoBehaviour
{
    public GameObject nextNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            if (nextNode)
            {
                other.gameObject.GetComponent<NPC>().SetNode(nextNode);
            }
            else
            {
                Debug.Log("turn");
                other.gameObject.GetComponent<NPC>().Turn();
            }
        }
    }
}
