using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    [SerializeField] float moveZAmount = 72.5f;
    // Start is called before the first frame update
    public GameObject roadSection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            Instantiate(roadSection, new Vector3(0f, 0f, moveZAmount), Quaternion.identity);
        }
    }
}
