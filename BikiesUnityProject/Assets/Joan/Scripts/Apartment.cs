using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : CityElement
{
    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        area.SetActive(true);
    }
}
