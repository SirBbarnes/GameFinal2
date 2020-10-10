using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Control : MonoBehaviour
{

    public GameObject[] NPC;
    bool spawn;
    int randNum;

    Transform spawnPoint;
    Transform destPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("spawnPoint").transform;
        destPoint = GameObject.FindGameObjectWithTag("stopPoint").transform;
    }

    void Update()
    {
        if (spawn == false)
        {
            randNum = Random.Range(0, 5);

            Instantiate(NPC[randNum], spawnPoint);
            spawn = true;
        }

        if (spawn == true)
        {
            if (NPC[randNum].transform.position == destPoint.transform.position)
            {
                Destroy(NPC[randNum]);
                spawn = false;
            }
        }
    }

}
