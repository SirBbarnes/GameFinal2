using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class PlateScript : NetworkComponent
{
    public Transform placement;

    private float submissionHeight;

    private RecipeManagerScript managerScript;
    private RecipeScript recipeScript;
    private NetwrokTransform netTransformTemp;

    public bool checkRecipe;

    ParticleSystem myParticle;

    AudioSource myAudio;
    

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
            managerScript = GameObject.FindGameObjectWithTag("manager").GetComponent<RecipeManagerScript>();

            submissionHeight = GameObject.FindGameObjectWithTag("sub").transform.position.y;
        }
        if (IsClient)
        {

        }
        while (IsServer)
        {
            if(this.transform.position.y > submissionHeight)
            {
                myParticle.Play();
                myAudio.Play();
                
                managerScript.RecieveOrder(recipeScript.ToString());

                MyCore.NetDestroyObject(NetId);
                //destroy
            }

            yield return new WaitForSeconds(0.2f); //potentially slower
        }
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
                    if (!recipeScript.isBeingHeld)
                    {
                        Transform temp = other.transform.root;
                        //recipeScript = temp.GetComponent<RecipeScript>();
                        temp.position = placement.position;
                        temp.rotation = placement.rotation;

                        netTransformTemp = temp.GetComponent<NetwrokTransform>();
                        netTransformTemp.SetPos(placement.position);
                        netTransformTemp.SetRot(placement.rotation.eulerAngles);
                        // no update sent??
                        temp.SetParent(this.transform);
                    }
                }
                //Debug.Log("cup on");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
