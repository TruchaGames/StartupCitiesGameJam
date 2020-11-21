using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class economy_tester : MonoBehaviour
{

    public EconomyManager econ_manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.B))
        {
           if(econ_manager.BuyBikeStation())
           Debug.Log("Bought a bike station");
           else
           Debug.Log("Could'nt buy a bike station");

        }
    }
}
