using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class CupNet : NetworkComponent, Actions
{
    public Transform coffeeStuff;
    public Transform coffeeMax;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "A") == 0)
        {
            coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, coffeeMax.position, 0.001f);
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
    public void AddHeight()
    {
        //if (IsServer)
        //{
            //coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, coffeeMax.position, 0.001f);
            SendUpdate("A","X");

            Debug.Log("is pouring");
        //}
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
