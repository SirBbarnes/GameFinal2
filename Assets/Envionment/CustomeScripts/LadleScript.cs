using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class LadleScript : NetworkComponent
{
    public Transform coffeeStuff;
    public Transform max;
    private RecipeScript otherRecipeScript;
    private RecipeScript recipeScript;
    

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "D") == 0)
        {
            coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, max.position, 0.01f);
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
    public void OnTriggerStay(Collider other)
    {
        if (IsServer)
        {
            if (other.transform.root.CompareTag("item"))
            {
                otherRecipeScript = other.transform.root.GetComponent<RecipeScript>();
                if (otherRecipeScript != null)
                {
                    recipeScript.AppendHistory(otherRecipeScript.cookingHistory);
                }
                //Debug.Log("cup on");
            }
        }
    }
    public void AddHeight()
    {
        if (IsServer)
        {
            //coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, coffeeMax.position, 0.001f);
            SendUpdate("A", "X");
        }
        //Debug.Log(coffeeMax.position.y-coffeeStuff.position.y);

        //0.2 tolerance
        //Debug.Log("Hegiht"+ coffeHeight);
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
