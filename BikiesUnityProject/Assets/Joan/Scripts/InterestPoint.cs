﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPoint : CityElement
{
    // ~~~~~~~~ Provisional ~~~~~~~~
    //Provisional, will
    public float bikeParkingPeriod = 5.0f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        area.SetActive(true);
    }
}
