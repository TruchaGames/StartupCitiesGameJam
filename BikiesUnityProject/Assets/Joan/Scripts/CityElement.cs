using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityElement : MonoBehaviour
{ 
    public int radius = 0;

    public uint bikeCapacity = 20;
    public uint currentBikesAmount = 0;

    //Range of the element
    public GameObject area;

    //  -- When a point is activated from behind hidden or unactive we call this function
    public virtual void Activate() {}

    // Start is called before the first frame update
    void Start()
    {
        //Debug purpose: Update sphere size
        area.transform.localScale = new Vector3(radius, radius, radius);
        area.SetActive(false);
    }
}
