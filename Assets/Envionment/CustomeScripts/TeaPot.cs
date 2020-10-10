using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaPot : MonoBehaviour
{
    public Transform spout;

    private Coffee coffeeScript;
    private RecipeScript recipeScript;
    public RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckBelow());
    }

    public IEnumerator CheckBelow()
    {
        while (true)
        {
            Debug.DrawLine(spout.position, spout.position + -3f * Vector3.up, Color.red);

            if (Physics.Raycast(spout.position, -3f * Vector3.up, out hit))
            {
                if (hit.transform.root.tag == "item")
                {
                    recipeScript = hit.transform.root.GetComponent<RecipeScript>();
                    recipeScript.AddAction("POUR", "TEA");
                    

                    if (hit.collider.name == "coffee")
                        hit.collider.transform.root.GetComponent<Coffee>().AddHeight();
                }
                //Debug.Log(hit.collider.name);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
