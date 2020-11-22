using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityElement : MonoBehaviour
{
    protected CityManager cityManager;

    [Header("Lists of Nearby BikeStations")]
    public List<BikeStation> nearbyBikeStations = new List<BikeStation>();

    [Header("Radius Detection")]
    public float bikeStationDetectRadius = 5.0f;

    //Range of the element
    public GameObject area;

    protected bool ApartmentActive = false;

    //  -- When a point is activated from behind hidden or unactive we call this function
    public void Activate()
    {
        area.SetActive(true);
        ApartmentActive = true;
    }

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        Debug.Assert(cityManager != null, "GameObject <" + this.gameObject.name + "> is lacking a CityManager!");
    }

    // Start is called before the first frame update
    void Start()
    {
        //cityManager = FindObjectOfType<CityManager>();

        ////Debug purpose: Update sphere size
        //Vector3 scale = area.transform.localScale;
        //Vector3 pos = area.transform.position;
        //
        ////scale.y = Random.Range(1.1f, 7.82f);
        //scale.y = Random.Range(6.0f, 10.0f);
        //pos.y += scale.y/2;
        //
        //area.transform.position = pos;
        //area.transform.localScale = scale;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bikeStationDetectRadius);
    }
}
