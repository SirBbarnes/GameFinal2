using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class PotNet : NetworkComponent
{
    public Transform coffeeStuff;
    public Transform max;
    private RecipeScript otherRecipeScript;
    private RecipeScript recipeScript;
    private Vector3 min;

    public Transform spout;
    private RaycastHit hit;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "A") == 0)
        {
            coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, max.position, 0.5f);
            coffeeStuff.localScale += new Vector3(0.3f, 0, 0.3f); //.transform.localScale += new Vector3(1, 0, 1);
        }
        if (string.CompareOrdinal(flag, "B") == 0)
        {
            coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, min, 0.5f);
            coffeeStuff.localScale -= new Vector3(0.1f, 0, 0.1f); //.transform.localScale += new Vector3(1, 0, 1);
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
            this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

            min = coffeeStuff.position;
        }
        while (IsServer)
        {
            Debug.DrawLine(spout.position, spout.position + -3f * Vector3.up, Color.red);

            if (Physics.Raycast(spout.position, -3f * Vector3.up, out hit))
            {
                if (hit.transform.root.tag == "item")
                {
                    
                    otherRecipeScript = hit.transform.root.GetComponent<RecipeScript>();
                    if(otherRecipeScript != null)
                        otherRecipeScript.AppendHistory(recipeScript.cookingHistory);



                    if (hit.collider.name == "coffee" || hit.collider.name == "cup")
                    {
                        SendUpdate("B", "X");
                        hit.collider.transform.root.GetComponent<CupNet>().AddHeight();
                    }
                }
                //Debug.Log(hit.collider.name);
            }


            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            Transform temp = other.transform.root;

            if (temp.CompareTag("item"))
            {
                otherRecipeScript = temp.GetComponent<RecipeScript>();
                if (otherRecipeScript != null)
                {
                    recipeScript.AddAction("ADD", otherRecipeScript.ingredientName);

                    if (!(hit.collider.name == "coffee" || hit.collider.name == "cup"))
                    {
                        int otherNetId = temp.GetComponent<NetworkID>().NetId;

                        MyCore.NetDestroyObject(otherNetId);
                    }
                    //coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, max.position, 0.01f);
                    SendUpdate("A", "X");
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
