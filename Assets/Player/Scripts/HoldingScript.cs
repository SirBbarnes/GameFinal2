using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class HoldingScript : MonoBehaviour
{
    public FastIKFabric fastFabric;
    public Transform currentItem;

    public bool isHolding;
    public float rotSpeed;
    public float rotX;

    private bool alreadyHolding;

    public bool onServer;

    // Start is called before the first frame update
    void Start()
    {
        //fastFabric = transform.GetChild(0).GetComponent<FastIKFabric>();
        //handCollider = GetComponent<SphereCollider>();
        //StartCoroutine(ReadRotation());
        //isHolding = true;
        alreadyHolding = false;
        onServer = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (currentItem != null)
        {
            currentItem.transform.position = Vector3.Lerp(currentItem.transform.position, transform.TransformPoint(handCollider.center), 0.5f);
        }
        */
    }
    public void SetRot(float dir)
    {
        //Debug.DrawLine(transform.position, transform.position +ray, Color.green);
        //Debug.DrawLine(transform.position, transform.position + prevDir, Color.green);
        //Debug.DrawLine(transform.position, transform.position + Vector3.ProjectOnPlane(transform.position -ray, -transform.right), Color.red);

        //tempAngle = Vector3.Angle(prevDir, Vector3.ProjectOnPlane(transform.position -ray, -transform.right));
        //transform.rotation = Quaternion.AngleAxis(tempAngle, prevUp);
        //transform.Rotate(1f, transform.rotation.y, transform.rotation.z);
        //transform.RotateAround(1f, transform.rotation.y, transform.rotation.z);
        //Debug.Log("Val=" + Quaternion.Euler(tempAngle, 0, 0).ToString());
        //transform.rotation = Quaternion.Euler(tempAngle, 0, 0);
        //transform.rotation = Quaternion.FromToRotation(prevDir, Vector3.ProjectOnPlane(transform.position - ray, -transform.right));
        //Debug.Log("Val=" + transform.rotation.ToString());

        //Debug.Log(prevDir);
        //Debug.Log(Vector3.Angle(transform.up, Vector3.ProjectOnPlane(ray, prevUp)));
        //prevUp = transform.up;
        //transform.rotation *= Quaternion.Euler(3f, 0, 0);
        //prevUp = transform.up;
        rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad * dir;
        //Debug.Log(rotX);
        transform.parent.RotateAround(transform.right, rotX);
    }
    public void LetGo()
    {
        if (onServer)
        {
            isHolding = false;
            if (currentItem != null)
            {
                if (currentItem.transform.parent != null)
                {
                    if (currentItem.GetComponent<RecipeScript>() != null)
                        currentItem.GetComponent<RecipeScript>().isBeingHeld = false;

                    currentItem.parent = null;

                    alreadyHolding = false;
                }
            }
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if (onServer)
        {
            if (other.transform.root.CompareTag("item") && isHolding)
            {
                if (!alreadyHolding)
                {
                    currentItem = other.transform.root;

                    if (currentItem.GetComponent<RecipeScript>() != null)
                        currentItem.GetComponent<RecipeScript>().isBeingHeld = true;
                    //Debug.Log("hand hit");
                    currentItem.SetParent(this.transform);
                    //other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position,transform.TransformPoint(handCollider.center), 0.5f);
                    //Debug.Log(other.gameObject.name);
                    alreadyHolding = true;
                }
            }
        }
    }
}
