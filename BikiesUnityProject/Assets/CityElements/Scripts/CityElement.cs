using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityElement : MonoBehaviour
{
    // Range of the element
    public GameObject area;

    // List of Bike Stations nearby
    public List<GameObject> m_BikeStationsInRange;

    //  -- When a point is activated from behind hidden or unactive we call this function
    public void Activate()
    {
        area.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug purpose: Update sphere size
        Vector3 scale = area.transform.localScale;
        Vector3 pos = area.transform.position;

        //scale.y = Random.Range(1.1f, 7.82f);
        scale.y = Random.Range(6.0f, 25.0f);
        pos.y += scale.y/2;

        area.transform.position = pos;
        area.transform.localScale = scale;

        area.SetActive(false);
    }

    // Add Bike Stations to list
    public void AddBikeStation(GameObject bike_station)
    {
        m_BikeStationsInRange.Add(bike_station);
    }

    // Remove Bike Stations to list
    public void RemoveBikeStation(GameObject bike_station)
    {
        m_BikeStationsInRange.Remove(bike_station);
    }
    public void RemoveBikeStation(int bike_station_index)
    {
        m_BikeStationsInRange.RemoveAt(bike_station_index);
    }
}
