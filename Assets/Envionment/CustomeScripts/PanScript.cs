using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class PanScript : NetworkComponent
{
    public Transform spawnPos;

    private RecipeScript otherRecipeScript;
    private RecipeScript recipeScript;

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
            recipeScript = GetComponent<RecipeScript>();
        }
        if (IsClient)
        {

        }
        //while (IsServer)
        //{



            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }
    /*
    public void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            Transform temp = other.transform.root;

            if (other.name == "egg")
            {
                otherRecipeScript = temp.GetComponent<RecipeScript>();
                if (otherRecipeScript != null)
                {
                    if (!otherRecipeScript.isBeingHeld)
                    {
                        recipeScript.AddAction("BREAK", otherRecipeScript.ingredientName);

                        //if (!(hit.collider.name == "coffee" || hit.collider.name == "cup"))
                        //{

                        MyCore.NetCreateObject(19, NetId, spawnPos.position); //fryed egg

                        int otherNetId = temp.GetComponent<NetworkID>().NetId;

                        MyCore.NetDestroyObject(otherNetId);
                    }
                    
                    //}

                }
                //Debug.Log("cup on");
            }
            //Debug.Log(temp.name);
        }
    }
    */
    public void OnCollisionEnter(Collision other)
    {
        if (IsServer)
        {
            Transform temp = other.collider.transform.root;

            if (temp.name == "egg")
            {
                otherRecipeScript = temp.GetComponent<RecipeScript>();
                if (otherRecipeScript != null)
                {
                    if (!otherRecipeScript.isBeingHeld)
                    {
                        recipeScript.AddAction("BREAK", otherRecipeScript.ingredientName);

                        //if (!(hit.collider.name == "coffee" || hit.collider.name == "cup"))
                        //{

                        MyCore.NetCreateObject(19, NetId, spawnPos.position); //fryed egg

                        int otherNetId = temp.GetComponent<NetworkID>().NetId;

                        MyCore.NetDestroyObject(otherNetId);
                    }

                    //}

                }
                //Debug.Log("cup on");
            }
            //Debug.Log(temp.name);
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
