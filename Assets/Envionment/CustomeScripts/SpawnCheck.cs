using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheck : MonoBehaviour
{
    public bool isOccupied;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.CompareTag("item"))
        {
            isOccupied = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("item"))
        {
            isOccupied = false;
        }
    }
}
