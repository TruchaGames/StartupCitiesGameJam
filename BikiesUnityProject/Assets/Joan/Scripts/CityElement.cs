using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityElement : MonoBehaviour
{ 
    public int radious = 0;

    public uint bikeCapacity = 20;
    public uint currentBikesAmount = 0;

    //Range of the element
    public GameObject area;

    protected bool active = false;

    //  -- When a point is activated from behind hidden or unactive we call this function
    public virtual void Activate() { }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
