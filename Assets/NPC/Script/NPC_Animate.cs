using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Animate : MonoBehaviour
{
    public Animator myAnim;
    public UnityEngine.AI.NavMeshAgent myNav;
    public Transform[] movePoints;
    int destPoint;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = gameObject.GetComponent<Animator>();
        myNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myNav.autoBraking = false;
        NextPoint();
    }

    void NextPoint()
    {
        if (movePoints.Length == 0)
            return;

        myNav.destination = movePoints[destPoint].position;
        destPoint = (destPoint + 1) % movePoints.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
