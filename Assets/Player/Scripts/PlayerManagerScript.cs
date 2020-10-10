using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;


public class PlayerManagerScript : NetworkComponent
{
    public GameObject cookPanel;
    public GameObject instructionPanel;
    public GameObject menuPanel;

    public Text orderText;
    public Text statusText;

    public bool playerIsLeft;
    public bool playerIsRight;
    public int myNetId;

    private bool isReset;
    private bool leftButtonHit;
    private bool rightButtonHit;

    public GameObject pollManager;
    public Lobby lobbyScript;
    public RecipeManagerScript managerScript;

    public Button leftButton;
    public Button rightButton;


    public GameObject temp;
    public HandNetwork handScript;

    private Dictionary<string, int> foodDictionary;
    private Dictionary<string, int> cookwareDictionary;
    private Transform spawnPos;
    private bool otherOrder;
    private SpawnManagerScript spawnManager;

    public Sprite Image1;
    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "LFT") == 0)
        {
            playerIsLeft = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "RIT") == 0)
        {
            playerIsRight = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "LTS") == 0)
        {
            leftButton.interactable = false;
            //leftButtonHit = true;
            //rightButton.interactable = false;
        }
        if (string.CompareOrdinal(flag, "RTS") == 0)
        {
            rightButton.interactable = false;
            //rightButtonHit = true;
        }
        if (string.CompareOrdinal(flag, "RET") == 0)
        {
            isReset = true;
        }
        if (string.CompareOrdinal(flag, "NID") == 0)
        {
            myNetId = int.Parse(value);
        }
        if (string.CompareOrdinal(flag, "GET") == 0)
        {
            // and unopcupied spot or too many of that item
            // will need item manager
            Debug.Log("Item being sent:"+value);
            Debug.Log("Food:" + spawnManager.canFoodSpawn);
            Debug.Log("Cook:" + spawnManager.canCookSpawn);
            if (foodDictionary.ContainsKey(value))
            {
                if (spawnManager.canFoodSpawn)
                {
                    MyCore.NetCreateObject(foodDictionary[value], NetId, spawnManager.foodSpawn);
                }
            }
            else if(cookwareDictionary.ContainsKey(value))
            {
                if (spawnManager.canCookSpawn)
                {
                    MyCore.NetCreateObject(cookwareDictionary[value], NetId, spawnManager.cookSpawn);
                }
            }
        }
        if (string.CompareOrdinal(flag, "MKE") == 0)
        {
            //new Vector3(Random.Range(5f, -5f), Random.Range(1f, -1f), Random.Range(5f, -5f))

            temp = MyCore.NetCreateObject(1, myNetId, new Vector3(0,0,0));
            //temp.gameObject.tag = "hero";
            handScript = temp.GetComponent<HandNetwork>();
            handScript.SetHand(playerIsLeft, playerIsRight);
            //get available spawn slot
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsLocalPlayer)
        {
            this.gameObject.tag = "playerManager";
            yield return new WaitForSeconds(0.4f);//wait for scene

            pollManager = GameObject.FindGameObjectWithTag("poll");
            lobbyScript = pollManager.GetComponent<Lobby>();
            lobbyScript.AddClient(NetId);


            managerScript = GameObject.FindGameObjectWithTag("manager").GetComponent<RecipeManagerScript>();

            int tempId = MyCore.LocalPlayerId;
            //Debug.Log("My core local="+tempId);
            SendCommand("NID", tempId.ToString());
            otherOrder = true;

            this.transform.GetChild(0).gameObject.SetActive(true); // turn on UI
        }
        if (IsServer)
        {
            playerIsLeft = false;
            playerIsRight = false;

            //for string to prefab transfrom array
            foodDictionary = new Dictionary<string, int>();
            foodDictionary.Add("Watermelon", 9);
            foodDictionary.Add("Egg", 18);
            foodDictionary.Add("Toast", 20);
            foodDictionary.Add("Daikon", 16);
            foodDictionary.Add("Tea", 10);

            cookwareDictionary = new Dictionary<string, int>();
            cookwareDictionary.Add("Cup", 8);
            cookwareDictionary.Add("Plate", 11);
            cookwareDictionary.Add("Pan", 17);
            cookwareDictionary.Add("Pot", 15);

            //spawnPos = GameObject.FindGameObjectWithTag("spawn").transform;
            spawnManager = GameObject.FindGameObjectWithTag("spawnManger").GetComponent<SpawnManagerScript>();

        }
        //true update
        while (IsServer)
        {
            if (playerIsLeft)
                SendUpdate("LTS", "X");
            else if (playerIsRight)
                SendUpdate("RTS", "X");



            yield return new WaitForSeconds(0.5f); //potentially slower
        }
    }
    public void OKAY()
    {
        if (IsLocalPlayer)
        {
            lobbyScript.CountOkayClient(NetId);
            
            SendCommand("MKE", "X");

            StartCoroutine(ChangeUI());
            StartCoroutine(CheckOrders());

            //Debug.Log("hit start");
        }
    }
    public void LeftButtonHit()
    {
        if (IsLocalPlayer)
        {
            SendCommand("LFT", true.ToString());

            //leftButton.interactable = false;
            rightButton.interactable = false;

            leftButton.GetComponent<Image>().sprite = Image1;
            //rightButton.enabled = false;
        }
        if (IsClient)
        {
            //rightButton.enabled = false;
            Debug.Log("Left buttone status:" + leftButton.interactable);
            Debug.Log("Right buttone status:" + rightButton.interactable);
        }
    }
    public void RightButtonHit()
    {
        if (IsLocalPlayer)
        {
            SendCommand("RIT", true.ToString());

            leftButton.interactable = false;
            //rightButton.interactable = false;

            rightButton.GetComponent<Image>().sprite = Image1;
        }
        if (IsClient)
        {
            Debug.Log("Left buttone status:" + leftButton.interactable);
            Debug.Log("Right buttone status:" + rightButton.interactable);
        }
    }
    public void ResetButtonHit() //unfinished
    {
        if (IsLocalPlayer)
        {
            SendCommand("RET", "X");
            if (playerIsLeft)
            {
                SendCommand("LFT", false.ToString());
                rightButton.interactable = true;
            }
            if (playerIsRight)
            {
                SendCommand("RIT", false.ToString());
                leftButton.interactable = true;
            }

            playerIsLeft = false;
            playerIsRight = false;
        }
        if (IsClient)
        {
            Debug.Log("Left buttone status:" + leftButton.interactable);
            Debug.Log("Right buttone status:" + rightButton.interactable);
        }
    }

    public IEnumerator ChangeUI()
    {
        if (IsLocalPlayer)
        {
            yield return new WaitUntil(() => lobbyScript.ready);

            this.transform.GetChild(0).gameObject.SetActive(false); // turn off UI
            SetPanel(0);
            //menuPanel.SetActive(true);

            orderText.text = ParseOrder(managerScript.currentOrder);
            //Debug.Log(orderText.text);
        }
    }
    public IEnumerator CheckOrders()
    {
        while(IsLocalPlayer)
        {
            yield return new WaitUntil(() => otherOrder == managerScript.nextOrder);

            orderText.text = ParseOrder(managerScript.currentOrder);
            statusText.text = managerScript.status;

            otherOrder = !otherOrder;
            //UISwitch();
        }
    }
    public void CookPanel(int p)
    {
        if (IsLocalPlayer)
        {
            SetPanel(p);
        }
    }
    public void ToMenuPanel(int p)
    {
        if (IsLocalPlayer)
        {
            SetPanel(p);
        }
    }
    public void GetItem(Text textRef)
    {
        if (IsLocalPlayer)
        {
            SendCommand("GET",textRef.text);
            //Debug.Log(textRef.text);
        }
    }
    public void Instructions(int p)
    {
        if (IsLocalPlayer)
        {
            SetPanel(p);
        }
    }
    public string ParseOrder(string order)
    {
        if (order == "DONE")
        {
            return order;
        }
        else
        {
            try
            {
                string actualOrder = "";

                string[] dish = order.Split(';');
                string[] actions = dish[1].Split('+');
                string[] stuff = actions[1].Split('_');

                actualOrder = actions[0].Trim('_') + " " + actions[1] + " to " + dish[0];

                return actualOrder;
            }
            catch
            {
                return "PARSE ERROR";
            }
        }

    }
    public void SetPanel(int panel)
    {
        switch(panel)
        {
            case 0:
                menuPanel.SetActive(true);
                cookPanel.SetActive(false);
                instructionPanel.SetActive(false);
                break;
            case 1:
                menuPanel.SetActive(false);
                cookPanel.SetActive(true);
                instructionPanel.SetActive(false);
                break;
            case 2:
                menuPanel.SetActive(false);
                cookPanel.SetActive(false);
                instructionPanel.SetActive(true);
                break;
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
