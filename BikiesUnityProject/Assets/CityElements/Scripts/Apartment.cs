using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : CityElement
{    
    [Header("Cyclist Instance")]
    public GameObject Cyclist;

    [Header("Radius of Cyclist Spawn")]
    public float SpawnRadius = 5.0f;

    [Header("DEBUG")]
    public GameObject final_dest;

    // --- Temporary ---
    public float TimeToSpawn = 0.0f;
    float m_SpawiningTimer = 0.0f;

    bool spawn = true;

    // Update is called once per frame
    void Update()
    {
        m_SpawiningTimer += Time.deltaTime;
        if (m_SpawiningTimer > TimeToSpawn && spawn)
        {
            m_SpawiningTimer = 0.0f;
            SpawnCyclist();
            spawn = false;
        }
    }

    private void SpawnCyclist()
    {
        // Instantiate Cyclist around nearby, set position and destination
        AIAgent new_cyclist = GameObject.Instantiate(Cyclist).GetComponent<AIAgent>();
        Vector2 random_circle = Random.insideUnitCircle * SpawnRadius;
        new_cyclist.gameObject.transform.position = gameObject.transform.position + new Vector3(random_circle.x, 0.0f, random_circle.y);

        // Pick a random destination (interest point)
        //int IPIndex = Random.Range(0, cityManager.activeInterestPoints.Count - 1);
        //new_cyclist.FinalDestination = cityManager.activeInterestPoints[IPIndex].gameObject;
        new_cyclist.SetDestination(final_dest.gameObject);

        // Look for the nearest bike station with bikes
        int bikestation_index = -1;
        if (nearbyBikeStations.Count > 0)
        {
            int i = 1;
            float distance = (nearbyBikeStations[0].transform.position - transform.position).magnitude;
            foreach (BikeStation bike_st in nearbyBikeStations)
            {
                if (bike_st.bikeStock > 0)
                {
                    float new_distance = (bike_st.transform.position - transform.position).magnitude;
                    if (new_distance < distance)
                    {
                        bikestation_index = i;
                        distance = new_distance;
                    }
                }
                
                ++i;
            }
        }

        // Pass the bike station to the cyclist (or keep it null if -1)
        if (nearbyBikeStations.Count > 0)
        {
            BikeStation bike_station = nearbyBikeStations[0];
            new_cyclist.ChangeDestination(bike_station.gameObject, AIAgent.AGENT_STATUS.WALKING, bike_station.ArriveRadius);
        }
        
        //if (bikestation_index != -1)
        //    new_cyclist.ChangeDestination(m_BikeStationsInRange[bikestation_index], AIAgent.AGENT_STATUS.WALKING);
        //else
        //    new_cyclist.ChangeDestination(null, AIAgent.AGENT_STATUS.APT_WAIT);
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
                bikeStation.nearbyApartments.Add(this);

                ++nodesConnected;
            }
        }

        return nodesConnected;
    }
}
