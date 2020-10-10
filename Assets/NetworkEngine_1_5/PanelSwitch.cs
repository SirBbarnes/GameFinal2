using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : MonoBehaviour
{
    public GameObject p1;

    public void DisablePanel()
    {
        p1.SetActive(false);
    }
}
