using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class RecipeManagerScript : NetworkComponent
{
    public bool nextOrder;
    public string currentOrder;
    public string status;
    private List<string> orders;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "ORDER") == 0)
        {
            Debug.Log("Order recieved:" + value);
            currentOrder = value;
        }
        if (string.CompareOrdinal(flag, "CHANGE") == 0)
        {
            nextOrder = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "STATUS") == 0)
        {
            status = value;
        }

    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
            nextOrder = false;
            currentOrder = "tea cup;_+POUR_TEA_";
        }
        if (IsServer)
        {

            orders = new List<string>();

            //here are our orders
            orders.Add("tea cup;_+POUR_TEA_");
            orders.Add("tea cup;_+ADD_daikon_");
            orders.Add("toastSpread;_+ADD_FRYEDEGG_");
            
            nextOrder = false;
            
            currentOrder = orders[0];
            
            //Debug.Log(orders.Count);
            SendUpdate("ORDER", currentOrder);

            //submissionHeight = GameObject.FindGameObjectWithTag("sub").transform.position.y;
        }
        //while (IsServer)
        //{
            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }

    public void RecieveOrder(string myOrder)
    {
        if (IsServer)
        {
            if (orders.Count != 0)
            {
                if(myOrder == orders[0])
                {
                    status = "CORRECT!";
                }
                else
                {
                    status = "NOT MY ORDER";
                }
                SendUpdate("STATUS", status);

                orders.RemoveAt(0);
                currentOrder = orders[0];
                SendUpdate("ORDER", currentOrder);
                

            }
            else
            {
                SendUpdate("ORDER", "DONE");
                status = "THANKS FOR PLAYING";
                SendUpdate("STATUS", status);
            }
            nextOrder = !nextOrder;
            SendUpdate("CHANGE", nextOrder.ToString());
            
            //Debug.Log(myOrder);
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
