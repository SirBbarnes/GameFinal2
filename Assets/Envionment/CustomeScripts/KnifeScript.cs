using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class KnifeScript : NetworkComponent
{
    private RecipeScript recipeScript;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "D") == 0)
        {

        }

    }

    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
        {
            
        }
        if (IsClient)
        {

        }
        //while (IsServer)
        //{

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (IsServer)
        {
            if (other.transform.root.CompareTag("item"))
            {
                recipeScript = other.transform.root.GetComponent<RecipeScript>();
                if (recipeScript != null)
                {
                    //apply knife func
                }
                //Debug.Log("cup on");
            }
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
