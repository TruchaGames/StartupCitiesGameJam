﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : CityElement
{    
    [Header("Cyclist Instance")]
    public GameObject Cyclist;

    [Header("Cyclist Spawn")]
    public float SpawnRadius = 5.0f;
    public float timeToSpawn = 5.0f;
    float cyclistSpawnedAt = 0.0f;

    public Queue<AIAgent> cyclistsWaiting = new Queue<AIAgent>();

    void Start()
    {
        cyclistSpawnedAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn Cyclists
        if (Time.time - cyclistSpawnedAt > timeToSpawn)
        {
            cyclistSpawnedAt = Time.time;
            SpawnCyclist();
        }

        // Send Cyclists to a BikeStation
        if (cyclistsWaiting.Count > 0)
        {
            foreach (BikeStation bikeStation in nearbyBikeStations)
            {
                if (bikeStation.bikeStock > 0)  //IMPROVE: Search for closed instead of picking first available
                {
                    AIAgent cyclist = cyclistsWaiting.Dequeue();
                    cyclist.SetDestination(bikeStation.gameObject, bikeStation.ArriveRadius);
                    cyclist.AgentStatus = AIAgent.AGENT_STATUS.WALKING;
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
