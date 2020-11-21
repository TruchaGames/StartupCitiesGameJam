using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeBase : MonoBehaviour
{
    [Header("Bikes")]
    public uint bikeStock = 5;

    [Header("Radius Detection")]
    public float bikeBaseDetectRadius = 0.0f;
    public float apartmentDetectRadius = 0.0f;
    public float interestPointDetectRadius = 0.0f;

    [Header("Lists of Nearby Nodes")]
    public List<BikeBase> nearbyBikeBases = new List<BikeBase>();
    public List<Apartment> nearbyApartments = new List<Apartment>();
    public List<InterestPoint> nearbyInterestPoints = new List<InterestPoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    uint SearchBikeBases()
    {
        //// 1. Remove current nodes listed
        //float nodesConnected = 0.0f;
        //nearbyBikeBases.Clear();

        //// 2. Iterate list of all existing bike bases
        //foreach (BikeBase bikeBase in cityManager.activeBikeBases)
        //{
        //    // 3. Do a A->B vector from this base to each existing base
        //    float x_distance = bikeBase.gameObject.transform.position.x - gameObject.transform.position.x;
        //    float y_distance = bikeBase.gameObject.transform.position.y - gameObject.transform.position.y;

        //    // 4. If magnitude of A->B is <= to radius, then add to correspodant list
        //    if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= bikeBaseDetectRadius)
        //    {
        //        nearbyBikeBases.Add(bikeBase);

        //        // 5. Also, add yourself to the list of other newly connected nodes
        //        bikeBase.nearbyBikeBases.Add(this);

        //        ++nodesConnected;
        //    }
        //}

        return 0;
    }

    uint SearchApartements()
    {
        return 0;
    }

    uint SearchInterestPoint()
    {
        return 0;
    }
}
