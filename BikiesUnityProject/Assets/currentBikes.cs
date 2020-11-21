using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class currentBikes : MonoBehaviour
{

    public List<Image> bikeSlots = new List<Image>();
    public List<Image> freeSlots = new List<Image>();
    public TextMeshProUGUI bikeText;
    public TextMeshProUGUI bikeCost;

    public uint maxBikes;
    public uint bikeStock;

    public EconomyManager economyManager;

    public BikeStation bikeStation;

    // public Image[] bikeSlots;

    // Start is called before the first frame update
    void Awake()
    {

        bikeStation = GetComponentInParent<BikeStation>();
        maxBikes = bikeStation.maxBikes;
        bikeStock = bikeStation.bikeStock;

       // bikeText.SetText(bikeStock + "/" + maxBikes);
    }

    // Update is called once per frame
    void Update()
    {
        maxBikes = bikeStation.maxBikes;
        bikeStock = bikeStation.bikeStock;

        if (bikeText != null)
            bikeText.SetText(bikeStock + "/" + maxBikes);

        if (bikeCost != null)
            bikeCost.SetText(economyManager.bike_individual_cost.ToString());
    }

    public void AddBike()
    {
        for (int i = 0; i < bikeSlots.Count; i++)
        {
            if (bikeSlots[i].tag == "Empty")
            {
                bikeSlots[i].color = Color.red;
                bikeSlots[i].tag = "Full";
                return;
            }
        }

    }

}
