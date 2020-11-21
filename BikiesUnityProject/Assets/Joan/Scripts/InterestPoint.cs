using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPoint : CityElement
{
    // ~~~~~~~~ Provisional ~~~~~~~~
    //Provisional, will
    public float bikeParkingPeriod = 5.0f;
    

    // ~~~~~~~~ Provisional ~~~~~~~~


    // Start is called before the first frame update
    void Start()
    {
        //Debug purpose: Update sphere size
        area.transform.transform.localScale = new Vector3(radious, radious, radious);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        active = true;

        GetComponentInChildren<MeshRenderer>().enabled = true;
    }
}
