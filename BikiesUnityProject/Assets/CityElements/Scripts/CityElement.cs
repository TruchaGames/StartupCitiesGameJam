using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityElement : MonoBehaviour
{
    public int radius = 0;

    [Header("ADD City Manager GO HERE!")]
    public CityManager cityManager;

    [Header("Lists of Nearby BikeStations")]
    public List<BikeStation> nearbyBikeStations;

    [Header("Radius Detection")]
    public float bikeStationDetectRadius = 0.0f;

    //Range of the element
    public GameObject area;

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

    protected bool InsideRadius(Vector3 nearbyElement, float radius)
    {
        float x_distance = nearbyElement.x - gameObject.transform.position.x;
        float y_distance = nearbyElement.z - gameObject.transform.position.z;

        if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= radius)
            return true;
        else
            return false;
    }
}
