using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeStation : MonoBehaviour
{
    [Header("ADD City Manager GO HERE!")]
    public CityManager cityManager;

    [Header("Bikes")]
    public uint bikeStock = 5;
    public float bikePickupCooldown = 1; // In seconds
    float bikePickedAt = 0.0f;

    [Header("Radius Detection")]
    //public float bikeStationDetectRadius = 0.0f;
    public float apartmentDetectRadius = 0.0f;
    public float interestPointDetectRadius = 0.0f;

    [Header("Lists of Nearby Nodes")]
    //public List<BikeStation> nearbyBikeStations = new List<BikeStation>();
    public List<Apartment> nearbyApartments = new List<Apartment>();
    public List<InterestPoint> nearbyInterestPoints = new List<InterestPoint>();

    [Header("Queue of Waiting Cyclists")]
    public Queue<AIAgent> waitingCyclists = new Queue<AIAgent>();

    [Header("Cyclist Arrive Radius")]
    public float ArriveRadius = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        EstablishConnections();
    }

    //Building variables
    private bool constructable = true;
    private uint collisions = 0; //Amount of colliders that don't allow the construction of the station

    // Update is called once per frame
    void Update()
    {

        //foreach (BikeStation BikeStation in nearbyBikeStations)
        //{
        //    Debug.DrawLine(transform.position, BikeStation.transform.position);
        //};

        if (Time.time - bikePickedAt > bikePickupCooldown && bikeStock > 0 && waitingCyclists.Count > 0)
        {
            AIAgent cyclist = waitingCyclists.Dequeue();

            //LUCHO-TODO: Metelo el estado que sea y mándalo a pastar al ciclista a su bike destination.
            InterestPoint interest_pt = cyclist.FinalDestination.GetComponent<InterestPoint>();

            int bikestation_index = -1;
            if(interest_pt.nearbyBikeStations.Count > 0)
            {
                int i = 1;
                float distance_to_objective = (interest_pt.nearbyBikeStations[0].transform.position - cyclist.FinalDestination.transform.position).magnitude;

                foreach(BikeStation bike_st in interest_pt.nearbyBikeStations)
                {
                    //if(bike_st.bikeStock) -- If not full
                    float new_distance = (bike_st.transform.position - cyclist.FinalDestination.transform.position).magnitude;
                    if(new_distance < distance_to_objective)
                    {
                        bikestation_index = i;
                        distance_to_objective = new_distance;
                    }

                    ++i;
                }
            }

            if (interest_pt.nearbyBikeStations.Count > 0)
            {
                BikeStation bike_station = interest_pt.nearbyBikeStations[0];
                cyclist.ChangeDestination(bike_station.gameObject, AIAgent.AGENT_STATUS.TRAVELLING, bike_station.ArriveRadius);
            }

            --bikeStock;
            bikePickedAt = Time.time;
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.0f, transform.position.z), bikeStationDetectRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.0f, transform.position.z), apartmentDetectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.0f, transform.position.z), interestPointDetectRadius);

        //------------------------------------------------------

        foreach (Apartment BikeStation in nearbyApartments)
        {
            Debug.DrawLine(transform.position, BikeStation.transform.position);
        };

        foreach (InterestPoint BikeStation in nearbyInterestPoints)
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

    // PUBLIC: Request to connect to nearby nodes.
    public bool EstablishConnections()
    {
        //ConnectBikeStations();
        ConnectApartments();
        ConnectInterestPoints();

        return true;
    }

    // PUBLIC: Request to disconnect from all nodes.
    public void RemoveConnections()
    {
        //foreach (BikeStation it in nearbyBikeStations)
        //    it.nearbyBikeStations.Remove(this);

        //foreach (Apartment it in nearbyApartments)    // TODO: Create lists in appropiate places
        //    it.nearbyBikeStations.Remove(this);

        //foreach (InterestPoint it in nearbyInterestPoints)    // TODO: Create lists in appropiate places
        //    it.nearbyBikeStations.Remove(this);

        //nearbyBikeStations.Clear();
        nearbyApartments.Clear();
        nearbyInterestPoints.Clear();
    }

    bool InsideRadius(Vector3 nearbyElement, float radius)
    {
        float x_distance = nearbyElement.x - gameObject.transform.position.x;
        float y_distance = nearbyElement.z - gameObject.transform.position.z;

        if (Mathf.Sqrt(Mathf.Pow(x_distance, 2) + Mathf.Pow(y_distance, 2)) <= radius)
            return true;
        else
            return false;
    }

    //uint ConnectBikeStations()
    //{
    //    uint nodesConnected = 0;

    //    // 1. Iterate list of all existing bike bases
    //    foreach (BikeStation bikeStation in cityManager.bikeStations)
    //    {
    //        // 2. Do a A->B vector from this base to each existing base
    //        if (InsideRadius(bikeStation.transform.position, bikeStationDetectRadius))
    //        {
    //            // 3. If magnitude of A->B is <= to radius, then add to correspodant list
    //            nearbyBikeStations.Add(bikeStation);

    //            // 4. Also, add yourself to the list of other newly connected node
    //            bikeStation.nearbyBikeStations.Add(this);

    //            ++nodesConnected;
    //        }
    //    }

    //    return nodesConnected;
    //}

    uint ConnectApartments()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        foreach (Apartment apartment in cityManager.activeApartments)
        {
            // 2. Do a A->B vector from this base to each existing base
            if (InsideRadius(apartment.transform.position, apartmentDetectRadius))
            {
                // 3. If magnitude of A->B is <= to radius, then add to correspodant list
                nearbyApartments.Add(apartment);

                // 4. Also, add yourself to the list of other newly connected nodes
                apartment.nearbyBikeStations.Add(this);

                ++nodesConnected;
            }
        }

        return nodesConnected;
    }

    uint ConnectInterestPoints()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        foreach (InterestPoint iPoint in cityManager.activeInterestPoints)
        {
            // 2. Do a A->B vector from this base to each existing base
            if (InsideRadius(iPoint.transform.position, interestPointDetectRadius))
            {
                // 3. If magnitude of A->B is <= to radius, then add to correspodant list
                nearbyInterestPoints.Add(iPoint);

                // 4. Also, add yourself to the list of other newly connected nodes
                iPoint.nearbyBikeStations.Add(this);

                ++nodesConnected;
            }
        }

        return nodesConnected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Not constructable" || other.gameObject.tag == "City Element")
            collisions++;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Not constructable" || other.gameObject.tag == "City Element")
            collisions--;

    }

    public bool IsConstructable()
    {
        return collisions == 0;
    }
}
