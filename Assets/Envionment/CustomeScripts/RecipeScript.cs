using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeScript : MonoBehaviour
{
    public Dictionary<string, List<string>> cookingHistory;
    public bool addLock;
    public bool isBeingHeld;

    public string ingredientName;

    // Start is called before the first frame update
    void Start()
    {
        addLock = true;

        cookingHistory = new Dictionary<string, List<string>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddAction(string action,string ingredient)
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
    public void AppendHistory(Dictionary<string, List<string>> otherDict)
    {
        foreach (KeyValuePair<string, List<string>> entry in otherDict)
        {
            if (entry.Key != null)
            {
                for (int i = 0; i < entry.Value.Count; i += 1)
                    AddAction(entry.Key, entry.Value[i]);
            }

        }
    }
    public override string ToString()
    {
        string temp = "";
        foreach (KeyValuePair<string, List<string>> entry in cookingHistory)
        {
            temp += "+"+ entry.Key+"_";
            for (int i=0; i < entry.Value.Count; i += 1)
                temp += entry.Value[i]+"_";

        }
        return ingredientName + ";_" + temp;
    }
}
