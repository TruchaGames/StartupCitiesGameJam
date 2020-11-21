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
        Vector3 scale = area.transform.localScale;
        Vector3 pos = area.transform.position;

        //scale.y = Random.Range(1.1f, 7.82f);
        scale.y = Random.Range(0.3f, 5.0f);
        pos.y += scale.y/2;

        area.transform.position = pos;
        area.transform.localScale = scale;

        area.SetActive(false);
    }
}
