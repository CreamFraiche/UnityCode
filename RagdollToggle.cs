using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    public CapsuleCollider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<CapsuleCollider>();
        foreach (CapsuleCollider collider in colliders)
            collider.enabled = false;

        foreach (BoxCollider collider in GetComponentsInChildren<BoxCollider>())
            collider.enabled = false;

        foreach (SphereCollider collider in GetComponentsInChildren<SphereCollider>())
            collider.enabled = false;

        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void OnEnable()
    {
        foreach (CapsuleCollider collider in colliders)
            collider.enabled = true;
    }
}
