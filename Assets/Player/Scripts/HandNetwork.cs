using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class HandNetwork : NetworkComponent
{
    public bool isLeft;
    public bool isRight;

    public float moveScale;
    public float tolerance;
    public float scrollScale;

    public GameObject chef;
    public ChefScript chefScript;

    public Transform camPos;
    public Transform camTargetLeft;
    public Transform camTargetRight;
    private Camera cam;
    private Vector3 camPositionClient;
    private Vector3 camTargetClient;

    public Transform leftTarget;
    public Transform rightTarget;

    public Vector3 ray;
    public Vector3 prevRay;
    private Vector3 tempRayLeft;
    private Vector3 tempRayRight;
    private Vector3 tempProjL;
    private Vector3 tempProjR;
    private Vector3 dhSum;

    private float handHeightClient;//for scroll
    private float handHeightLeft;
    private float handHeightRight;
    private float sqrTolerance;

    private bool leftIsHold;
    private bool rightIsHold;

    private Ray rayDest;
    private RaycastHit hit;
    private int mask;
    private int otherNetId;

    private float speed = 5.0f;
    private float yaw;
    private float pitch;


    public override void HandleMessage(string flag, string value)
    {
        if (string.CompareOrdinal(flag, "LEFT") == 0)
        {
            isLeft = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "RIGHT") == 0)
        {
            isRight = bool.Parse(value);
        }
        if (string.CompareOrdinal(flag, "RAYL") == 0 && !chefScript.dualHold)
        {
            char[] remove = { '(', ')' };
            string[] data = value.Trim(remove).Split(',');
            tempRayLeft = new Vector3(
                            float.Parse(data[0]),
                            float.Parse(data[1]),
                            float.Parse(data[2])
                                );

            tempProjL = -Vector3.ProjectOnPlane(camTargetLeft.position - tempRayLeft, -camTargetLeft.up) * moveScale;

            if(!chefScript.dualHold)
                leftTarget.position = Vector3.Lerp(leftTarget.position, tempProjL + camTargetLeft.position, 0.5f);

            //leftTarget.position = Vector3.Lerp(leftTarget.position, leftTarget.position + leftTarget.up * handHeightLeft, 0.5f);

                //leftTarget.position = Vector3.Lerp(leftTarget.position, leftTarget.position + leftTarget.up * handHeightLeft, 0.5f);


        }
        if (string.CompareOrdinal(flag, "RAYR") == 0)
        {
            char[] remove = { '(', ')' };
            string[] data = value.Trim(remove).Split(',');
            tempRayRight = new Vector3(
                            float.Parse(data[0]),
                            float.Parse(data[1]),
                            float.Parse(data[2])
                                );

            tempProjR = -Vector3.ProjectOnPlane(camTargetRight.position - tempRayRight, -camTargetRight.up) * moveScale;

            if (!chefScript.dualHold)
                rightTarget.position = Vector3.Lerp(rightTarget.position, tempProjR + camTargetRight.position, 0.5f);

        }

        if (string.CompareOrdinal(flag, "UPL") == 0)
        {
            handHeightLeft = float.Parse(value);
            leftTarget.position = Vector3.Lerp(leftTarget.position, leftTarget.position + leftTarget.up * handHeightLeft, 0.5f);
            camTargetLeft.position = Vector3.Lerp(camTargetLeft.position, camTargetLeft.position + camTargetLeft.up * handHeightLeft, 0.5f);

        }
        if (string.CompareOrdinal(flag, "UPR") == 0)
        {
            handHeightRight = float.Parse(value);
            rightTarget.position = Vector3.Lerp(rightTarget.position, rightTarget.position + rightTarget.up * handHeightRight, 0.5f);
            camTargetRight.position = Vector3.Lerp(camTargetRight.position, camTargetRight.position + camTargetRight.up * handHeightRight, 0.5f);
        }
        if (string.CompareOrdinal(flag, "CAMPOS") == 0)
        {
            char[] remove = { '(', ')' };
            string[] data = value.Trim(remove).Split(',');
            camPositionClient = new Vector3(
                                float.Parse(data[0]),
                                float.Parse(data[1]),
                                float.Parse(data[2])
                                );
            
        }
        if (string.CompareOrdinal(flag, "CAMTAR") == 0)
        {
            char[] remove = { '(', ')' };
            string[] data = value.Trim(remove).Split(',');
            camTargetClient = new Vector3(
                                float.Parse(data[0]),
                                float.Parse(data[1]),
                                float.Parse(data[2])
                                );

        }

        if (string.CompareOrdinal(flag, "HL") == 0)
        {
            leftIsHold = bool.Parse(value);
            if (leftIsHold)
                chefScript.HoldLeft();
            else
                chefScript.LeftLetGo();
        }
        if (string.CompareOrdinal(flag, "HR") == 0)
        {
            rightIsHold = bool.Parse(value);
            if (rightIsHold)
                chefScript.HoldRight();
            else
            {
                chefScript.RightLetGo();

                //Debug.Log("letting go");
            }
        }
        if (string.CompareOrdinal(flag, "DEST") == 0)
        {
            MyCore.NetDestroyObject(int.Parse(value));
        }

        //turning later
        /*
        if(string.CompareOrdinal(flag, "TL") == 0)
        {
            chefScript.TurnLeft(bool.Parse(value));
        }
        */
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsLocalPlayer)
        {
            yield return new WaitUntil(()=> isLeft || isRight);

            if(isLeft)
                StartCoroutine(LeftHand());
            if(isRight)
                StartCoroutine(RightHand());

            yield return new WaitForSeconds(0.4f);


            Camera.main.transform.position = camPositionClient; 
            Camera.main.transform.LookAt(camTargetClient);

            mask = LayerMask.GetMask("Kitchen");

        }
        if (IsServer)
        {
            chef = GameObject.FindGameObjectWithTag("chef");
            chefScript = chef.GetComponent<ChefScript>();

            camPos = chefScript.cameraPosition;
            SendUpdate("CAMPOS", chefScript.cameraPosition.position.ToString("F4"));
            camTargetLeft = chefScript.cameraTargetL;
            SendUpdate("CAMTAR", chefScript.cameraTargetL.position.ToString("F4"));
            camTargetRight = chefScript.cameraTargetR;



            leftTarget = chefScript.leftChefTransform;
            rightTarget = chefScript.rightChefTransform;

            StartCoroutine(CombineDualHold());

            handHeightLeft = 0f;
        }
        //while (IsLocalPlayer)
        //{


            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
       // }
    }

    public IEnumerator LeftHand()
    {
        if (IsLocalPlayer)
        {
            prevRay = ray;
            cam = Camera.main;
            sqrTolerance = tolerance * tolerance;

            Camera.main.transform.LookAt(camTargetClient);


        }
        while (IsLocalPlayer)
        {
            ray = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camPositionClient.z + cam.nearClipPlane));
            Debug.DrawLine(camPositionClient, ray + camPositionClient, Color.green);

            if ((prevRay - ray).sqrMagnitude > sqrTolerance)
            {
                SendCommand("RAYL", ray.ToString("F3"));
                //Debug.Log("moving ray");
                prevRay = ray;
            }

            handHeightClient = Input.mouseScrollDelta.y; //needs fixing
            //if(handHeightClient != 0)
            //{
                handHeightClient *= scrollScale * -1f;
                SendCommand("UPL", handHeightClient.ToString());
            //}

            if (Input.GetMouseButton(0))
            {
                SendCommand("HL", true.ToString());
                //leftHolding.isHolding = true;
                //Debug.Log("holding");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SendCommand("HL", false.ToString());
                //leftHolding.LetGo();
                //Debug.Log("let go");
            }

            
            if (Input.GetMouseButton(1))
            {
                changeCamera();
            }

            if (Input.GetMouseButtonUp(1))
            {
                snapCamera();
            }
            

            if (Input.GetMouseButtonDown(2))
            {
                rayDest = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayDest, out hit, 10f, mask))
                {
                    //Debug.Log("Client dest:"+hit.collider.name);
                    otherNetId = hit.collider.transform.root.GetComponent<NetworkID>().NetId;
                    //Debug.Log("Client dest:" + otherNetId);
                    SendCommand("DEST", otherNetId.ToString());
                    //Destroy(hit.collider.gameObject);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendCommand("HL", false.ToString());
            }
            //Camera.main.transform.LookAt(camTargetClient);

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        }
    }
    public IEnumerator RightHand()
    {
        if (IsLocalPlayer)
        {
            prevRay = ray;
            cam = Camera.main;
            sqrTolerance = tolerance * tolerance;

            Camera.main.transform.LookAt(camTargetClient);
        }
        while (IsLocalPlayer)
        {
            ray = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camPositionClient.z + cam.nearClipPlane));
            Debug.DrawLine(camPositionClient, ray + camPositionClient, Color.green);

            if ((prevRay - ray).sqrMagnitude > sqrTolerance)
            {
                SendCommand("RAYR", ray.ToString("F3"));
                //Debug.Log("moving ray");
                prevRay = ray;
            }

            handHeightClient = Input.mouseScrollDelta.y;
            //if (handHeightClient != 0)
            //{
                handHeightClient *= scrollScale * -1f;
                SendCommand("UPR", handHeightClient.ToString());
            //}

            if (Input.GetMouseButton(0))
            {
                SendCommand("HR", true.ToString());
                //leftHolding.isHolding = true;
                //Debug.Log("holding");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SendCommand("HR", false.ToString());
                //leftHolding.LetGo();
                //Debug.Log("let go");
            }

            if (Input.GetMouseButton(1))
            {
                changeCamera();
            }

            if (Input.GetMouseButtonUp(1))
            {
                snapCamera();
            }

            if (Input.GetMouseButtonDown(2))
            {
                rayDest = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayDest, out hit, 10f, mask))
                {
                    //Debug.Log("Client dest:"+hit.collider.name);
                    otherNetId = hit.collider.transform.root.GetComponent<NetworkID>().NetId;
                    //Debug.Log("Client dest:" + otherNetId);
                    SendCommand("DEST", otherNetId.ToString());
                    //Destroy(hit.collider.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendCommand("HR", false.ToString());
            }
            //Camera.main.transform.LookAt(camTargetClient);

            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        }
    }

    public IEnumerator CombineDualHold()
    {
        while(IsServer)
        {
            yield return new WaitUntil(() => chefScript.dualHold);

            dhSum = tempProjL + tempProjR + (camTargetLeft.position + camTargetRight.position) * 0.5f;
            if (chefScript.dualHoldTransform != null)
            {
                chefScript.dualHoldTransform.position = Vector3.Lerp(chefScript.dualHoldTransform.position, dhSum, 0.5f);
                chefScript.dualHoldTransform.position = Vector3.Lerp(chefScript.dualHoldTransform.position,
                                                chefScript.dualHoldTransform.position+ chefScript.dualHoldTransform.up*0.5f *(handHeightLeft+handHeightRight), 0.5f);
            }

            /*
            leftTarget.position = Vector3.Lerp(leftTarget.position, leftTarget.position + leftTarget.up * handHeightLeft, 0.5f);
            camTargetLeft.position = Vector3.Lerp(camTargetLeft.position, camTargetLeft.position + camTargetLeft.up * handHeightLeft, 0.5f);
             */



            //Debug.Log("R:"+tempProjR.ToString());
        }
    }

    public void SetHand(bool leftHand, bool rightHand)
    {
        //if (IsServer)
        //{
            isLeft = leftHand;
            isRight = rightHand;

            SendUpdate("LEFT", leftHand.ToString());
            SendUpdate("RIGHT", rightHand.ToString());
        
        //}
    }

    public void changeCamera()
    {
        yaw += speed * Input.GetAxis("Mouse X");
        pitch -= speed * Input.GetAxis("Mouse Y");


        cam.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    public void  snapCamera()
    {
        Camera.main.transform.LookAt(camTargetClient);

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
