using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using DitzelGames.FastIK;

public class HoldingNetwork : NetworkComponent
{
    public FastIKFabric fastFabric;
    public GameObject currentItem;

    public bool isHolding;
    public float rotSpeed;
    public float rotX;

    public override void HandleMessage(string flag, string value)
    {

    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {

        }
        if (IsServer)
        {
            //fastFabric = transform.parent.GetChild(0).GetComponent<FastIKFabric>(); //hacky
        }
        //while (IsLocalPlayer)
        //{


        yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
                                                             // }
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
