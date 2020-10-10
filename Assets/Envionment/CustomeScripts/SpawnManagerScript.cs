using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class SpawnManagerScript : NetworkComponent
{
    private GameObject[] spawnPositions;

    private SpawnCheck foodSpawnCheck;
    private SpawnCheck cookwareSpawnCheck;

    public bool canFoodSpawn;
    public bool canCookSpawn;

    public Vector3 foodSpawn;
    public Vector3 cookSpawn;


    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "A") == 0)
        {

        }

    }

    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
        {
            spawnPositions =  GameObject.FindGameObjectsWithTag("spawn");

            //checking for height to assign spawn check script
            if(spawnPositions[0].transform.position.y > spawnPositions[1].transform.position.y)
            {
                foodSpawn = spawnPositions[0].transform.position;
                cookSpawn = spawnPositions[1].transform.position;

                foodSpawnCheck = spawnPositions[0].GetComponent<SpawnCheck>();
                cookwareSpawnCheck = spawnPositions[1].GetComponent<SpawnCheck>();
            }
            else
            {
                foodSpawn = spawnPositions[1].transform.position;
                cookSpawn = spawnPositions[0].transform.position;

                foodSpawnCheck = spawnPositions[1].GetComponent<SpawnCheck>();
                cookwareSpawnCheck = spawnPositions[0].GetComponent<SpawnCheck>();
            }

            canCookSpawn = false;
            canFoodSpawn = false;
        }
        if (IsClient)
        {

        }
        while (IsServer)
        {

            canFoodSpawn = !foodSpawnCheck.isOccupied;
            canCookSpawn = !cookwareSpawnCheck.isOccupied;

            yield return new WaitForSeconds(0.4f); //potentially slower
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
