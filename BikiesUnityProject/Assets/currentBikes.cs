using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class currentBikes : MonoBehaviour
{

    public List<Image> bikeSlots = new List<Image>();
    public List<Image> freeSlots = new List<Image>();

   // public Image[] bikeSlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBike()
    {
        for (int i = 0; i < bikeSlots.Count; i++)
        {
            if(bikeSlots[i].tag == "Empty")
            {
                bikeSlots[i].color = Color.red;
                bikeSlots[i].tag = "Full";
                return;
            }
        }

    }

}
