using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeStation : MonoBehaviour
{
    [Header("Bikes")]
    public uint bikeStock = 5;

    [Header("Radius Detection")]
    public float BikeStationDetectRadius = 0.0f;
    public float apartmentDetectRadius = 0.0f;
    public float interestPointDetectRadius = 0.0f;

    [Header("Lists of Nearby Nodes")]
    public List<BikeStation> nearbyBikeStations = new List<BikeStation>();
    public List<Apartment> nearbyApartments = new List<Apartment>();
    public List<InterestPoint> nearbyInterestPoints = new List<InterestPoint>();

    // Start is called before the first frame update
    void Start()
    {
        ConnectBikeStations();
        //ConnectApartments();
        //ConnectInterestPoints();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BikeStation BikeStation in nearbyBikeStations)
        {
            Debug.DrawLine(transform.position, BikeStation.transform.position);
        };
    }

    // Player actions
    /*
     * 1. Create bike base -> New base has no connections, connects with all nearby nodes and viceversa
     * 2. Reposition bike base -> Connections of bike base are removed, so are the connection of all nearby nodes to this bike base
     * 3. Remove bike base -> Connections to this bike base are removed from all other nodes
     * 4. Transport bikes from base to base -> Transferred stock of bikes from a place to the other
     */

    // Element Interactions
    /* 1. The apartment tells the bike base that someone is going to use a bike to go to x place.
     * 2. Bike bases send
     * 
     * 
     * 
     */

    uint ConnectBikeStations()
    {
        uint nodesConnected = 0;

        //// 1. Iterate list of all existing bike bases
        //foreach (BikeStation BikeStation in cityManager.activeBikeStations)
        //{
        //    // 2. Do a A->B vector from this base to each existing base
        //    float x_distance = BikeStation.gameObject.transform.position.x - gameObject.transform.position.x;
        //    float y_distance = BikeStation.gameObject.transform.position.y - gameObject.transform.position.y;

        //    // 3. If magnitude of A->B is <= to radius, then add to correspodant list
        //    if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= BikeStationDetectRadius)
        //    {
        //        nearbyBikeStations.Add(BikeStation);

        //        // 4. Also, add yourself to the list of other newly connected nodes
        //        BikeStation.nearbyBikeStations.Add(this);

        //        ++nodesConnected;
        //    }
        //}

        return nodesConnected;
    }

    uint ConnectApartments()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        //foreach (Apartment apartment in cityManager.activeApartments)
        //foreach (Apartment apartment in cityManager.activeApartments)
        //{
        //     2. Do a A->B vector from this base to each existing base
        //    float x_distance = apartment.gameObject.transform.position.x - gameObject.transform.position.x;
        //    float y_distance = apartment.gameObject.transform.position.y - gameObject.transform.position.y;

        //     3. If magnitude of A->B is <= to radius, then add to correspodant list
        //    if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= BikeStationDetectRadius)
        //    {
        //        nearbyApartments.Add(apartment);

        //         4. Also, add yourself to the list of other newly connected nodes
        //        apartment.nearbyBikeStations.Add(this);

        //        ++nodesConnected;
        //    }
        //}

        return nodesConnected;
    }

    uint ConnectInterestPoints()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        //foreach (InterestPoint iPoint in cityManager.activeInterestPoints)
        //{
        //    // 2. Do a A->B vector from this base to each existing base
        //    float x_distance = iPoint.gameObject.transform.position.x - gameObject.transform.position.x;
        //    float y_distance = iPoint.gameObject.transform.position.y - gameObject.transform.position.y;

        //    // 3. If magnitude of A->B is <= to radius, then add to correspodant list
        //    if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= BikeStationDetectRadius)
        //    {
        //        nearbyInterestPoints.Add(iPoint);

        //        // 4. Also, add yourself to the list of other newly connected nodes
        //        iPoint.nearbyBikeStations.Add(this);

        //        ++nodesConnected;
        //    }
        //}

        return nodesConnected;
    }
}
