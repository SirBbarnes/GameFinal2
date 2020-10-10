using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class NetwrokTransform : NetworkComponent
{
    public Vector3 prevPos = Vector3.zero;
    public Vector3 prevRot = Vector3.zero;

    private Vector3 tempPos;
    private Vector3 tempRot;

    public override void HandleMessage(string flag, string value)
    {
        char[] remove = { '(', ')' };
        if (string.CompareOrdinal(flag, "POS") == 0)
        {
            string[] data = value.Trim(remove).Split(',');
            tempPos = new Vector3(
                          float.Parse(data[0]),
                          float.Parse(data[1]),
                          float.Parse(data[2])
                                );
            this.transform.position = Vector3.Lerp(this.transform.position, tempPos, 0.5f);

        }
        if (string.CompareOrdinal(flag, "ROT") == 0)
        {
            string[] data = value.Trim(remove).Split(',');
            tempRot = new Vector3(
                          float.Parse(data[0]),
                          float.Parse(data[1]),
                          float.Parse(data[2])
                                );
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,Quaternion.Euler(tempRot),0.5f);
        }
    }
    public override IEnumerator SlowUpdate()
    {

        while (IsServer)
        {
            if ((prevPos - this.transform.position).magnitude > 0.01f)
            {
                SendUpdate("POS", this.transform.position.ToString());
                prevPos = this.transform.position;
            }
            if (prevRot != this.transform.rotation.eulerAngles)
            {
                SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString("F4"));
                prevRot = this.transform.rotation.eulerAngles;
            }

            if (IsDirty)
            {
                SendUpdate("POS", this.transform.position.ToString("F4"));
                SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(MyCore.MasterTimer); //potentially slower
        }
    }
    public void SetPos(Vector3 otherPos)
    {
        //if (IsServer)
        //{
            this.transform.position = otherPos;
            SendUpdate("POS", otherPos.ToString());
            prevPos = otherPos;
        //}
    }
    public void SetRot(Vector3 otherRot)
    {
        //if (IsServer)
        //{
            this.transform.rotation = Quaternion.Euler(otherRot);
            SendUpdate("ROT", otherRot.ToString("F4"));
            prevRot = otherRot;
       // }
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
