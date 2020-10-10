using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class DualHoldScript : NetworkComponent
{
    public Transform leftTarget;
    public Transform rightTarget;
    
    private Transform leftWrist;
    private HoldingScript holdingLeft;
    private Transform rightWrist;
    private HoldingScript holdingRight;

    private GameObject chef;
    private ChefScript chefScript;

    private bool leftIsHolding;
    private bool rightIsHolding;

    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "D") == 0)
        {
            //bothHolding = bool.Parse(value);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
        {
            leftWrist = GameObject.FindGameObjectWithTag("wristLeft").transform;
            rightWrist = GameObject.FindGameObjectWithTag("wristRight").transform;

            holdingLeft = leftWrist.GetComponent<HoldingScript>();
            holdingRight = rightWrist.GetComponent<HoldingScript>();

            leftTarget = GameObject.FindGameObjectWithTag("leftTarget").transform;
            rightTarget = GameObject.FindGameObjectWithTag("rightTarget").transform;
            

            chef = GameObject.FindGameObjectWithTag("chef");
            chefScript = chef.GetComponent<ChefScript>();

            leftIsHolding = false;
            rightIsHolding = false;

            StartCoroutine(CheckBothHolding());
        }
        if(IsClient)
        {
            yield return new WaitForSeconds(MyCore.MasterTimer * 5f);

            leftTarget = GameObject.FindGameObjectWithTag("leftTarget").transform;
            rightTarget = GameObject.FindGameObjectWithTag("rightTarget").transform;
            

            chef = GameObject.FindGameObjectWithTag("chef");
            chefScript = chef.GetComponent<ChefScript>();
        }
        //while (IsServer)
        //{
            //gripDistSqr = gripDist * gripDist;
            /*
            if ((leftWrist.position - leftGrip.position).sqrMagnitude < gripDistSqr && holdingLeft.isHolding)
            {
                Debug.DrawLine(leftGrip.position, leftWrist.position, Color.green);
            }
            if ((rightWrist.position - rightGrip.position).sqrMagnitude < gripDistSqr && holdingRight.isHolding)
            {
                Debug.DrawLine(rightGrip.position, rightWrist.position, Color.green);
            }

            bothHolding = ((leftWrist.position - leftGrip.position).sqrMagnitude < gripDistSqr && holdingLeft.isHolding) &&
                            ((rightWrist.position - rightGrip.position).sqrMagnitude < gripDistSqr && holdingRight.isHolding);
            */
            //SendUpdate("D", bothHolding.ToString());

            //if (bothHolding)
              //  this.transform.position = (leftWrist.position + rightWrist.position) * 0.5f;

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        //}
    }
    public IEnumerator CheckBothHolding()
    {
        while(IsServer)
        {
            //yield return new WaitUntil(() => bothHolding != tempBoth);
            yield return new WaitUntil(() => leftIsHolding && rightIsHolding);


            //Debug.Log("ass cheeks");
            chefScript.SetDualHold(true);

            //if (bothHolding)
            //{
            chefScript.dualHoldTransform = this.transform.root;
            leftTarget.SetParent(chefScript.dualHoldTransform);
            rightTarget.SetParent(chefScript.dualHoldTransform);
            //}
            //else
            //{

            yield return new WaitUntil(() => !leftIsHolding || !rightIsHolding);

            chefScript.SetDualHold(false);

            chefScript.dualHoldTransform = null;
            leftTarget.parent = null;
            rightTarget.parent = null;
            //}
            /*
            if (bothHolding)
                this.transform.SetParent(leftWrist.transform);
            else
                this.transform.parent = null;
                */
            //SendUpdate("DUH", bothHolding.ToString());
            //tempBoth = bothHolding;
            //Debug.Log(bothHolding);
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if(IsServer)
        {
            if (other.gameObject.CompareTag("wristLeft"))
            {
                leftIsHolding = holdingLeft.isHolding;
            }
            if (other.gameObject.CompareTag("wristRight"))
            {
                rightIsHolding = holdingRight.isHolding;
            }
            //Debug.Log(other.gameObject.name);
        }
        // if is client hight light shader
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
