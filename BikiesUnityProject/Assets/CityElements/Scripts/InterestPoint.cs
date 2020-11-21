using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPoint : CityElement
{
    // Update is called once per frame
    void Update()
    {

    }

    public uint ConnectBikeStations()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        foreach (BikeStation bikeStation in cityManager.bikeStations)
        {
            // 2. Do a A->B vector from this base to each existing base
            if (InsideRadius(bikeStation.transform.position, bikeStationDetectRadius))
            {
                // 3. If magnitude of A->B is <= to radius, then add to correspodant list
                nearbyBikeStations.Add(bikeStation);

                // 4. Also, add yourself to the list of other newly connected node
                bikeStation.nearbyInterestPoints.Add(this);

                ++nodesConnected;
            }
        }

        return nodesConnected;
    }
}
