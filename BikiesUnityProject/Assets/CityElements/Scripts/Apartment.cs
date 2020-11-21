using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : CityElement
{
    public GameObject cyclist;
    public GameObject bike_station;
    CityManager m_CityManager;

    // --- Temporary ---
    public float TimeToSpawn = 0.0f;
    float m_SpawiningTimer = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        m_CityManager = GameObject.FindObjectOfType<CityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_SpawiningTimer += Time.deltaTime;
        if (m_SpawiningTimer > TimeToSpawn)
        {
            m_SpawiningTimer = 0.0f;
            SpawnCyclist();
        }
    }

    private void SpawnCyclist()
    {
        SphereCollider sphere = gameObject.GetComponentInChildren<SphereCollider>();
        if (sphere != null)
        {
            // Look for the nearest bike station
            //int bikestation_index = -1;
            //if(m_BikeStationsInRange.Count > 0)
            //{
            //    bikestation_index = 0;
            //    Vector3 nearestBikeStation = m_BikeStationsInRange[0].transform.position;
            //
            //    for (int i = 1; m_BikeStationsInRange.Count > 1 && i < m_BikeStationsInRange.Count; ++i)
            //    {
            //        if (m_BikeStationsInRange[i].transform.position.magnitude < nearestBikeStation.magnitude)
            //        {
            //            nearestBikeStation = m_BikeStationsInRange[i].transform.position;
            //            bikestation_index = i;
            //        }
            //    }
            //}

            // Instantiate Cyclist nearby, set position and destination
            AIAgent new_cyclist = GameObject.Instantiate(cyclist).GetComponent<AIAgent>();
            
            Vector2 random_circle = Random.insideUnitCircle * 5.0f;
            new_cyclist.gameObject.transform.position = gameObject.transform.position + new Vector3(random_circle.x, 0.0f, random_circle.y);
            
            //int IPIndex = Random.Range(0, m_CityManager.activeInterestPoints.Count - 1);
            //new_cyclist.FinalDestination = m_CityManager.activeInterestPoints[IPIndex].gameObject;

            new_cyclist.ChangeDestination(bike_station, AIAgent.AGENT_STATUS.WALKING);

            // Pass the bike station to the cyclist (or keep it null if -1)
            //if (bikestation_index != -1)
            //    new_cyclist.ChangeDestination(m_BikeStationsInRange[bikestation_index], AIAgent.AGENT_STATUS.WALKING);
            //else
            //    new_cyclist.ChangeDestination(null, AIAgent.AGENT_STATUS.APT_WAIT);
        }
        else
            Debug.LogError("THE APARTMENT HAS NOT A SPHERE COLLIDER!");

    }    
}
