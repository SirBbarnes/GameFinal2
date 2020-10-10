using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class BreadScript : NetworkComponent
{
    public Transform rayPos;

    private RecipeScript otherRecipeScript;
    private RecipeScript recipeScript;

    private RaycastHit hit;
    private int mask;
    private bool reachedCond;

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
            mask = LayerMask.GetMask("Kitchen");
            reachedCond = true;
        }
        if (IsClient)
        {
            reachedCond = false;
        }
        while (IsServer && reachedCond)
        {
            //Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, 
            Debug.DrawLine(rayPos.position, rayPos.position + 3f * Vector3.up, Color.red);

            if (Physics.Raycast(rayPos.position, Vector3.up, out hit,3f,mask))
            {
                if(hit.collider.name == "fryedEgg(Clone)")
                {
                    hit.transform.SetParent(this.transform);

                    recipeScript.AddAction("ADD", "FRYEDEGG");

                    reachedCond = false;
                }

            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
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
