using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public GameObject obj;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item Collect()
    {
        obj.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        return item;
    }
}
