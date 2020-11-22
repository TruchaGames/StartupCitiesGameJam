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

    public uint maxBikes = new uint();
    public uint bikeStock = new uint();

    public EconomyManager economyManager;

    public BikeStation bikeStation;

    // public Image[] bikeSlots;

    // Start is called before the first frame update
    void Awake()
    {
        economyManager = FindObjectOfType<EconomyManager>();

        bikeStation = GetComponentInParent<BikeStation>();
        if (bikeStation != null)
        {
            maxBikes = bikeStation.maxBikes;
            bikeStock = bikeStation.bikeStock;
        }
        else
        {
            Debug.LogError("bikeStation of <currentBikes> was null!");
        }
        

       // bikeText.SetText(bikeStock + "/" + maxBikes);
    }

    // Update is called once per frame
    void Update()
    {
        if (bikeStation != null)
        {
            maxBikes = bikeStation.maxBikes;
            bikeStock = bikeStation.bikeStock;

            if (bikeText != null)
                bikeText.SetText(bikeStock + "/" + maxBikes);

            if (bikeCost != null)
                bikeCost.SetText(economyManager.bike_individual_cost.ToString());
        }
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
