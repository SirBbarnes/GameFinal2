using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class HandScript : MonoBehaviour
{
    public Transform camTarget;
    public Transform camPos;

    public bool IsLeft;
    public bool IsRight;
    public Transform targetLeftTransform;
    public Transform targetRightTransform;
    public Transform leftRest;

    public float handSpeed = 0.03f;
    public float moveScale;
    public float scrollScale;

    public GameObject leftWrist;
    public HoldingScript leftHolding;
    public GameObject rightWrist;
    public HoldingScript rightHolding;
    //public float cameraDist;


    private float armDist;//for scroll

    private Vector3 ray;
    private RaycastHit hit;
    private int mask;

    private Camera cam;
    private Vector3 mover;
    private Vector3 originalAxis;
    private float distanceToMaintain;
    private float tempAngle;
    private Vector3 prevDir;
    private Quaternion tempRot;

    void Start()
    {
        //targetTransform = target.transform;
        cam = Camera.main;
        StartCoroutine(MoveTarget());

        leftWrist = GameObject.FindGameObjectWithTag("leftWrist");
        leftHolding = leftWrist.GetComponent<HoldingScript>();
        rightWrist = GameObject.FindGameObjectWithTag("rightWrist");
        rightHolding = rightWrist.GetComponent<HoldingScript>();

        mask =  LayerMask.GetMask("Kitchen");

        //distanceToMaintain = (originalAxis - rightFoot.position).magnitude;

        //maxDistSqrd = maxDist * maxDist;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator MoveTarget()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ray = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,this.transform.position.z+cam.nearClipPlane));
            if (IsLeft)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    leftHolding.fastFabric.enabled = false;
                }
                else if(Input.GetMouseButtonUp(1))
                {
                    leftHolding.fastFabric.enabled = true;
                }
                if (Input.GetMouseButton(0))
                {
                    leftHolding.isHolding = true;
                    //Debug.Log("holding");
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    leftHolding.LetGo();
                    //Debug.Log("let go");
                }

                
                if (Input.GetMouseButton(1)) //rightclick
                {
                    //turn hand
                    //RotateAround(Vector3 point, Vector3 axis, float angle); 

                    //tempAngle = Vector3.Angle(Vector3.up, -Vector3.ProjectOnPlane(ray, prevDir));

                    //Debug.DrawLine(targetLeftTransform.position, targetLeftTransform.position- Vector3.ProjectOnPlane(ray, targetLeftTransform.forward), Color.red);

                    //targetLeftTransform.rotation *= Quaternion.AngleAxis(tempAngle, prevDir);

                    //Debug.DrawLine(targetLeftTransform.position, targetLeftTransform.position + prevRight, Color.green);
                    //targetTransform.Rotate(prevDir, tempAngle);
                    //targetLeftTransform.rotation = Quaternion.FromToRotation(prevRight, Vector3.Cross(prevDir, ray.direction));
                    //AngleAxis(float angle, Vector3 axis); 
                    //targetLeftTransform.rotation = Quaternion.AngleAxis(tempAngle, prevDir);

                    leftHolding.SetRot(1f);
                }
                else
                {
                    /*
                    armDist += Input.mouseScrollDelta.y * scale;
                    targetLeftTransform.position = Vector3.Lerp(targetLeftTransform.position, ray.origin + ray.direction * armDist, 0.5f);
                    Debug.DrawLine(ray.origin, targetLeftTransform.position, Color.green);

                    */
                    //Debug.DrawLine(camPos.position, ray+camPos.position, Color.magenta);
                    Debug.DrawLine(camTarget.position,   Vector3.ProjectOnPlane(ray, -camTarget.up), Color.red);
                    //Debug.DrawLine(camTarget.position, camTarget.position + camTarget.up, Color.green);

                    targetLeftTransform.position = Vector3.Lerp(targetLeftTransform.position,
                                                                Vector3.ProjectOnPlane(camTarget.position - ray, -camTarget.up) * moveScale + camPos.position, 0.5f);



                    armDist += Input.mouseScrollDelta.y * scrollScale * -1f;
                    targetLeftTransform.position = Vector3.Lerp(targetLeftTransform.position,
                                                       targetLeftTransform.position + targetLeftTransform.up * armDist, 0.5f);
                    //Debug.Log(prevDir);

                }

                //if (Input.mouseScrollDelta.y != 0)
                //{
                //}
                //Debug.DrawLine(camTarget.position, Vector3.ProjectOnPlane(camTarget.position-ray, camPos.forward) + camPos.position, Color.magenta);
            }
            else if (IsRight)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    rightHolding.fastFabric.enabled = false;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    rightHolding.fastFabric.enabled = true;
                }

                if (Input.GetMouseButton(0))
                {
                    rightHolding.isHolding = true;
                    //Debug.Log("holding");
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    rightHolding.LetGo();
                    //Debug.Log("let go");
                }

                if(Input.GetMouseButtonDown(2))
                {
                    Ray rayDest = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rayDest,out hit,10f,mask))
                    {
                        Debug.Log(hit.collider.name);
                        Destroy(hit.collider.gameObject);
                    }
                    //public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction); 
                }

                if (Input.GetMouseButton(1)) //rightclick
                {
                    //turn hand
                    //RotateAround(Vector3 point, Vector3 axis, float angle); 

                    //tempAngle = 2f;

                    //Debug.DrawLine(targetLeftTransform.position, targetLeftTransform.position + Vector3.Cross(prevDir, ray.direction), Color.red);
                    //Debug.DrawLine(targetLeftTransform.position, targetLeftTransform.position + prevRight, Color.green);
                    //targetTransform.Rotate(prevDir, tempAngle);
                    //targetLeftTransform.rotation = Quaternion.FromToRotation(prevRight, Vector3.Cross(prevDir, ray.direction));
                    //AngleAxis(float angle, Vector3 axis); 
                    rightHolding.SetRot(-1f);
                }
                else
                {
                    targetRightTransform.position = Vector3.Lerp(targetRightTransform.position,
                                                                Vector3.ProjectOnPlane(camTarget.position - ray, -camTarget.up) * moveScale + camPos.position, 0.5f);


                    armDist += Input.mouseScrollDelta.y * scrollScale * -1f;
                    targetRightTransform.position = Vector3.Lerp(targetRightTransform.position,
                                                       targetRightTransform.position + targetRightTransform.up * armDist, 0.5f);
                    //prevDir = ray;
                }
                //if(Input.mouseScrollDelta.y != 0)
                //{
                //}
            }
            //camNewPos = -1f * camFrwd * mainGuy.forward + camUp * mainGuy.up + mainGuy.position;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPos.position, 0.7f);
            Camera.main.transform.LookAt(camTarget.position);


            yield return new WaitForSeconds(handSpeed);
        }
    }
}