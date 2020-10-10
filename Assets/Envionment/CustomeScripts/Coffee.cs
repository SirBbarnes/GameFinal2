using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    public bool isPrepared;
    public Transform coffeeStuff;
    public Transform coffeeMax;
    // Start is called before the first frame update
    void Start()
    {
        //0.021
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddHeight()
    {

        coffeeStuff.position = Vector3.Lerp(coffeeStuff.position, coffeeMax.position, 0.001f);
        //Debug.Log(coffeeMax.position.y-coffeeStuff.position.y);

        //0.2 tolerance
        //Debug.Log("Hegiht"+ coffeHeight);
    }
}
