using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using DitzelGames.FastIK;

public class ChefScript : NetworkComponent
{
    public Transform cameraPosition;
    public Transform cameraTargetL;
    public Transform cameraTargetR;


    public Transform leftChefTransform;
    public Transform rightChefTransform;
    public Transform leftWrist;
    public Transform rightWrist;

    public GameObject wristLeft;
    public HoldingScript holdingLeft;
    public GameObject wristRight;
    public HoldingScript holdingRight;

    public Transform dualHoldTransform;


    //public Transform leftForearm;
    public Transform rightForearm;
    public GameObject leftFinger;
    public GameObject rightFinger;
    public FastIKFabric fabricLeft;
    public FastIKFabric fabricRight;

    public bool dualHold;

    private bool leftTurn;
    private bool isLeftHold;
    private bool isRightHold;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "WRIST") == 0)
        {
            /*
            leftWrist = GameObject.FindGameObjectWithTag("wristLeft").transform;
            leftWrist.SetParent(leftForearm, false);
            leftForearm.GetChild(0).transform.SetParent(leftWrist);
            */

            rightWrist = GameObject.FindGameObjectWithTag("wristRight").transform;
            rightWrist.SetParent(rightForearm, false);
            rightForearm.GetChild(0).transform.SetParent(rightWrist);
        }
        if (string.CompareOrdinal(flag, "SETHAND") == 0)
        {
            char[] remove = { '(', ')' };
            string[] data = value.Trim(remove).Split(',');
            Vector3 tempVec = new Vector3(
                                float.Parse(data[0]),
                                float.Parse(data[1]),
                                float.Parse(data[2])
                                );
            leftChefTransform.position = tempVec;
            rightChefTransform.position = tempVec;
        }
        if (string.CompareOrdinal(flag, "HLC") == 0)
        {
            isLeftHold = bool.Parse(value);
            if(isLeftHold)
                holdingLeft.isHolding = true;
            else
                holdingLeft.LetGo();
        }
        if (string.CompareOrdinal(flag, "HRC") == 0)
        {
            isRightHold = bool.Parse(value);
            if (isRightHold)
                holdingRight.isHolding = true;
            else
                holdingRight.LetGo();
        }
        if (string.CompareOrdinal(flag, "SLT") == 0)
        {
            leftTurn = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "R") == 0)
        {
            fabricLeft.enabled = leftTurn;
            holdingLeft.SetRot(float.Parse(value));
        }
        if (string.CompareOrdinal(flag, "DH") == 0)
        {
            dualHold = bool.Parse(value);
        }
        /*
        if (string.CompareOrdinal(flag, "HAND") == 0)
        {
            //GameObject[] tempArr = GameObject.FindGameObjectsWithTag("target");

            leftChefTransform = GameObject.FindGameObjectWithTag("leftTarget").transform;
            rightChefTransform = GameObject.FindGameObjectWithTag("rightTarget").transform;

            fabricLeft = leftFinger.GetComponent<FastIKFabric>();
            fabricLeft.Target = leftChefTransform;
            fabricRight = rightFinger.GetComponent<FastIKFabric>();
            fabricRight.Target = rightChefTransform;
        }
        */
    }
    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
        {
            //this.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);

            leftChefTransform = GameObject.FindGameObjectWithTag("leftTarget").transform;
            rightChefTransform = GameObject.FindGameObjectWithTag("rightTarget").transform;

            fabricLeft = leftFinger.GetComponent<FastIKFabric>();
            fabricLeft.Target = leftChefTransform;
            fabricRight= rightFinger.GetComponent<FastIKFabric>();
            fabricRight.Target = rightChefTransform;

            SendUpdate("HAND", "X");


            //leftWrist = GameObject.FindGameObjectWithTag("wristLeft").transform;
            //leftWrist.SetParent(leftForearm,false); 
            //leftForearm.GetChild(0).transform.SetParent(leftWrist);


            //rightWrist = GameObject.FindGameObjectWithTag("wristRight").transform;
            //rightWrist.SetParent(rightForearm,false);
            //rightForearm.GetChild(0).transform.SetParent(rightWrist);


            //SendUpdate("WRIST", "X");
            
            leftChefTransform.position = cameraTargetL.position;
            rightChefTransform.position = cameraTargetL.position;

            SendUpdate("SETHAND", cameraTargetL.position.ToString("F3"));

            holdingLeft = wristLeft.GetComponent<HoldingScript>();
            holdingRight = wristRight.GetComponent<HoldingScript>();

            holdingLeft.onServer = true;
            holdingRight.onServer = true;

        }
        if (IsClient)
        {
            yield return new WaitForSeconds(MyCore.MasterTimer*10);

            this.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);

            leftChefTransform = GameObject.FindGameObjectWithTag("leftTarget").transform;
            rightChefTransform = GameObject.FindGameObjectWithTag("rightTarget").transform;

            fabricLeft = leftFinger.GetComponent<FastIKFabric>();
            fabricLeft.Target = leftChefTransform;
            fabricRight = rightFinger.GetComponent<FastIKFabric>();
            fabricRight.Target = rightChefTransform;


            holdingLeft = wristLeft.GetComponent<HoldingScript>();
            holdingRight = wristRight.GetComponent<HoldingScript>();

        }
        //while (IsServer) //sphere collider equal srist position
        //{
            //if(holdingLeft.currentItem != null)
            //    Debug.Log(holdingLeft.currentItem.name);

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }
    public void HoldLeft()
    {
        holdingLeft.isHolding = true;
        SendUpdate("HLC", true.ToString());
        //Debug.Log("something being held");
    }
    public void LeftLetGo()
    {
        //Debug.Log("letting go");
        holdingLeft.LetGo();
        SendUpdate("HLC", false.ToString());
    }
    public void HoldRight()
    {
        holdingRight.isHolding = true;
        SendUpdate("HRC", true.ToString());
        //Debug.Log("something being held");
    }
    public void RightLetGo()
    {
        //Debug.Log("letting go");
        holdingRight.LetGo();
        SendUpdate("HRC", false.ToString());
    }
    public void TurnLeft(bool active)
    {
        fabricLeft.enabled = active;
        SendUpdate("SLT", active.ToString());
        holdingRight.SetRot(1f);
        SendUpdate("R", 1f.ToString());
    }
    public void SetDualHold(bool hold)
    {
        dualHold = hold;
        SendUpdate("DH", dualHold.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveCamera());
    }

    public IEnumerator moveCamera()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Change Camera");
        }

        yield return new WaitForSeconds(MyCore.MasterTimer);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
