using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Lobby : NetworkComponent
{
    //public int clientNumber=0;
    //public int clientTally=0;
    public GameObject player;
    public PlayerManagerScript playerScript;
    public ArrayList lobbyList;
    public bool ready;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "ADD") == 0)
        {
            int id = int.Parse(value);
            //Debug.Log("ADDED="+id.ToString());
            lobbyList.Add(id);
            Debug.Log("On ADD, LL=" + ArrayListToString(lobbyList));
            //Debug.Log("add gets called");
        }
        if (string.CompareOrdinal(flag, "OKY") == 0)
        {
            int id = int.Parse(value);
            //Debug.Log("OKYED="+id.ToString());
            lobbyList.Remove(id);

            Debug.Log("On OKY, LL=" + ArrayListToString(lobbyList));
        }
        if (string.CompareOrdinal(flag, "RDY") == 0)
        {
            ready = true;
        }
    }
    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
            ready = false;

            //StartCoroutine(GetPollCheck());
            yield return new WaitForSeconds(MyCore.MasterTimer*20);//wait for player

            player = GameObject.FindGameObjectWithTag("playerManager");

            playerScript = player.GetComponent<PlayerManagerScript>();


        }

        if (IsServer)
        {
            lobbyList = new ArrayList();
            StartCoroutine(CheckLobbyList());

            //this.transform.GetChild(0).gameObject.SetActive(false);
        }
        //while (true)
        //{

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }
    public void AddClient(int id)
    {
        //clientNumber += 1;
        //SendUpdate("ADD", clientNumber.ToString());
        if (IsClient)
        {
            SendCommand("ADD", id.ToString());
            Debug.Log("add client getting called=" + id);
        }
    }
    public void CountOkayClient(int id)
    {
        //clientTally += 1;
        if (IsClient)
        {
            SendCommand("OKY", id.ToString());
            Debug.Log("countoky client getting called=" + id);
        }
    }
    public IEnumerator CheckLobbyList()
    {
        if(IsServer)
        {
            Debug.Log("LL still zero");
            yield return new WaitUntil(() => lobbyList.Count != 0);
            Debug.Log("LL not zero anymore");
            yield return new WaitUntil(() => lobbyList.Count == 0);

            bool tmp = true;
            ready = true;
            SendUpdate("RDY", tmp.ToString());
            //Debug.Log("CheckLL rdy being called" + tmp);
        }
    }
    public string ArrayListToString(ArrayList myList)
    {
        string val = "";
        for (int i = 0; i < myList.Count; i += 1)
            val += myList[i].ToString() + ",";
        return val;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(IsClient)
        {
            this.gameObject.tag = "poll";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
