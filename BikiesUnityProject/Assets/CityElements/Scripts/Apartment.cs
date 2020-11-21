using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : CityElement
{    
    [Header("Cyclist Instance")]
    public GameObject Cyclist;

    [Header("Radius of Cyclist Spawn")]
    public float SpawnRadius = 5.0f;

    public Queue<AIAgent> cyclistsWaiting = new Queue<AIAgent>();

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

        if (cyclistsWaiting.Count > 0)
        {
            foreach (BikeStation bikeStation in nearbyBikeStations)
            {
                if (bikeStation.bikeStock > 0)  //IMPROVE: Search for closed instead of picking first available
                {
                    AIAgent cyclist = cyclistsWaiting.Dequeue();
                    cyclist.ChangeDestination(bikeStation.gameObject, AIAgent.AGENT_STATUS.WALKING, bikeStation.ArriveRadius);
                }
            }
        }
    }

    private void SpawnCyclist()
    {
        // 1. Instantiate Cyclist around nearby, set position and destination
        AIAgent new_cyclist = GameObject.Instantiate(Cyclist).GetComponent<AIAgent>();
        Vector2 random_circle = Random.insideUnitCircle * SpawnRadius;
        new_cyclist.gameObject.transform.position = gameObject.transform.position + new Vector3(random_circle.x, 0.0f, random_circle.y);

        // 2. Pick a random destination (interest point)
        int IPIndex = Random.Range(0, cityManager.activeInterestPoints.Count - 1);
        new_cyclist.finalDestination = cityManager.activeInterestPoints[IPIndex];

        // 3. Mark origin of cyclist
        new_cyclist.sourceApartment = this;
        cyclistsWaiting.Enqueue(new_cyclist);
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
