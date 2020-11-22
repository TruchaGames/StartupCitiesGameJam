using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BikeStation : MonoBehaviour
{
    [Header("Cyclist UI")]
    public GameObject cyclistsWaitingGO;
    public TextMeshProUGUI[] cyclistTypeAmountText;
    public Image[] cyclistTimerImg;
    uint[] cyclistTypeAmount = new uint[5];

    CityManager cityManager;
    EconomyManager economyManager;

    [Header("Bikes")]
    public uint maxBikes = 8;
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
    public Queue<AIAgent> cyclistsWaiting = new Queue<AIAgent>();
    public AIAgent[] cyclistWaitList;

    [Header("Cyclist Arrive Radius")]
    public float ArriveRadius = 5.0f;


    [Header("Audio Events")]
    public AK.Wwise.Event move_bike;

    public SpriteRenderer areaCircle;

    void Awake()
    {
        areaCircle = GetComponentInChildren<SpriteRenderer>();
        cityManager = FindObjectOfType<CityManager>();
        economyManager = FindObjectOfType<EconomyManager>();
        Debug.Assert(cityManager != null, "GameObject <" + this.gameObject.name + "> is lacking a CityManager!");
    }

    // Start is called before the first frame update
    void Start()
    {
        areaCircle.size = new Vector2(apartmentDetectRadius, apartmentDetectRadius);

        //EstablishConnections();   //Remain commented

        for (int i = 0; i < cyclistTypeAmount.Length; ++i)
        {
            cyclistTypeAmount[i] = 0;
        }
    }

    //Building variables
    private int collisions = 0; //Amount of colliders that don't allow the construction of the station

    // Update is called once per frame
    void Update()
    {
        if (IsConstructable() == true)
            areaCircle.color = new Color(153/255, 255 / 255, 156 / 255);
        else
            areaCircle.color = new Color(255 / 255, 79 / 255, 73 / 255);


        if (Time.time - bikePickedAt > bikePickupCooldown && bikeStock > 0 && cyclistsWaiting.Count > 0)    // TODO-UI: Show UI of amount of cyclists waiting in the station, their wanted destination, and the waiting time of each (use list = queue.ToList())
            OfferBikeToCyclist();

        for (InterestPoint.InterestPointType IP_type = InterestPoint.InterestPointType.IP_NONE + 1; IP_type != InterestPoint.InterestPointType.IP_MAX; ++IP_type)
        {
            foreach (AIAgent cyclist in cyclistWaitList)
            {
                if (cyclist.finalDestination.interestPointType == IP_type)
                {
                    RunCyclistTimer(cyclistTimerImg[(int)IP_type], cyclist);
                    break;
                }
            }
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

        //foreach (BikeStation BikeStation in nearbyBikeStations)
        //{
        //    Debug.DrawLine(transform.position, BikeStation.transform.position);
        //};

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

        //foreach (Apartment it in nearbyApartments)
        //    it.nearbyBikeStations.Remove(this);

        //foreach (InterestPoint it in nearbyInterestPoints)
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

    void OfferBikeToCyclist()
    {
        AIAgent cyclist = cyclistsWaiting.Dequeue();

        InterestPoint cyclistDestination = cyclist.finalDestination;
        bool foundStationWithSlots = false;

        foreach (BikeStation bikeStation in cyclistDestination.nearbyBikeStations)
        {
            if (bikeStation.bikeStock < bikeStation.maxBikes)
            {
                TakeBike(cyclist, bikeStation);
                foundStationWithSlots = true;
                break;
            }
        }

        if (!foundStationWithSlots)
        {
            if (cyclistDestination.nearbyBikeStations.Count > 0)
                TakeBike(cyclist, cyclistDestination.nearbyBikeStations[0]);
            else
                EnqueueCyclist(cyclist);
        }
    }

    void TakeBike(AIAgent cyclist, BikeStation stationDestination)
    {
        cyclist.SetDestination(stationDestination.gameObject, stationDestination.ArriveRadius);
        cyclist.AgentStatus = AIAgent.AGENT_STATUS.TRAVELLING;

        //Change visually a pedestrian to a bike!
        Agent_Bicycle agent_bicycle = cyclist.GetComponentInChildren<Agent_Bicycle>();
        agent_bicycle.unit_type = UNITTYPE.Bicycle;

        //Audio event for a cyclist starting to bike
        move_bike.Post(cyclist.transform.gameObject);

        economyManager.BikeRentIncome();
        --bikeStock;
        //TODO-UI: Show UI of bycicle removed from stock of station and money increased by rental?
        bikePickedAt = Time.time;
    }

    public void EnqueueCyclist(AIAgent cyclist)
    {
        cyclistsWaiting.Enqueue(cyclist);
        int it = (int)cyclist.finalDestination.interestPointType;
        ++cyclistTypeAmount[it];
        cyclistTypeAmountText[it].text = cyclistTypeAmount[it].ToString();
        cyclistWaitList = cyclistsWaiting.ToArray();

        if (cyclistsWaitingGO != null && cyclistsWaiting.Count == 1)
        {
            cyclistsWaitingGO.SetActive(true);
        }
    }

    public void DequeueCyclist(AIAgent cyclist)
    {
        cyclistsWaiting.Dequeue();
        int it = (int)cyclist.finalDestination.interestPointType;
        --cyclistTypeAmount[it];
        cyclistTypeAmountText[it].text = cyclistTypeAmount[it].ToString();
        cyclistWaitList = cyclistsWaiting.ToArray();

        if (cyclistsWaitingGO != null && cyclistsWaiting.Count == 0)
        {
            cyclistsWaitingGO.SetActive(false);
        }
    }

    void RunCyclistTimer(Image img, AIAgent cyclist)
    {
        float timePassed = Time.time - cyclist.startedWaitingAt;
        if (timePassed < cyclist.waitTimeLimit)
            img.fillAmount = (cyclist.waitTimeLimit - timePassed) / cyclist.waitTimeLimit;
    }
}
