using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class TeaNet : NetworkComponent
{
    public Transform spout;

    private Coffee coffeeScript;
    private RecipeScript recipeScript;
    public RaycastHit hit;

    bool isHeld;

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
        while (IsServer)
        {
            Debug.DrawLine(spout.position, spout.position + -3f * Vector3.up, Color.red);

            if (Physics.Raycast(spout.position, -3f * Vector3.up, out hit))
            {
                if (hit.transform.root.tag == "item")
                {
                    recipeScript = hit.transform.root.GetComponent<RecipeScript>();

                    if (recipeScript != null)
                        recipeScript.AddAction("POUR", "TEA");


                    if (hit.collider.name == "coffee" || hit.collider.name == "cup")
                        hit.collider.transform.root.GetComponent<CupNet>().AddHeight();
                }
                //Debug.Log(hit.collider.name);
            }
            yield return new WaitForSeconds(MyCore.MasterTimer); 
        }
        //if being held play effect
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
