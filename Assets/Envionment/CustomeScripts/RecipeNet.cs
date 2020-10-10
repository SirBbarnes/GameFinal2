using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class RecipeNet : NetworkComponent
{
    public bool isBeingHeld;

    public Dictionary<string, List<string>> cookingHistory;

    public override void HandleMessage(string flag, string value)
    {
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsLocalPlayer)
        {

        }
        if (IsServer)
        {
            cookingHistory = new Dictionary<string, List<string>>();
        }
        //while (IsLocalPlayer)
        //{


        yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
                                                             // }
    }
    public void AddAction(string action, string ingredient)
    {
        if (IsServer)
        {
            if (cookingHistory.ContainsKey(action))
            {
                if (!cookingHistory[action].Contains(ingredient))
                {
                    cookingHistory[action].Add(ingredient);
                    Debug.Log(action + " " + ingredient);
                }
            }
            else
            {
                List<string> tempList = new List<string>();
                tempList.Add(ingredient);
                cookingHistory.Add(action, tempList);
                Debug.Log(action + " " + ingredient);
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
