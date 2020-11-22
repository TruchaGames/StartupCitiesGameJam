using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    
    GameObject NextDestination;
    public Apartment sourceApartment;
    public InterestPoint finalDestination;

    NavMeshAgent m_Agent;

    [Header("Agent Patience")]
    public float waitTimeLimit = 10.0f;
    float startedWaitingAt = 0.0f;

    bool waitedForRecast = false;   // We'll use this for a small workaround so that Re-Cast has time to calculate

    // AI Status
    public enum AGENT_STATUS {
        NONE = -1,
        APT_WAIT,
        WALKING,
        BIKE_WAIT,
        TRAVELLING,
        ARRIVING
    };
    public AGENT_STATUS AgentStatus = AGENT_STATUS.NONE;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        startedWaitingAt = Time.time;
        AgentStatus = AGENT_STATUS.APT_WAIT;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();

        switch (AgentStatus)
        {
            // Agent waits around the apartement
            case AGENT_STATUS.APT_WAIT:
                if (Time.time - startedWaitingAt > waitTimeLimit)
                {
                    sourceApartment.cyclistsWaiting.Dequeue();
                    Destroy(gameObject);
                    //TODO-Lucho: Augmentar polució, eliminate agent, etc.
                }
                break;

            // Agent Walks from Apartment to Bike Station A
            case AGENT_STATUS.WALKING:
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Enqueue(this);
                    startedWaitingAt = Time.time;
                    AgentStatus = AGENT_STATUS.BIKE_WAIT;
                }
                break;

            // Agent waits around the bike station
            case AGENT_STATUS.BIKE_WAIT:
                if (Time.time - startedWaitingAt > waitTimeLimit)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Dequeue();
                    Destroy(gameObject);
                    //TODO-Lucho: Augmentar polució, eliminate agent, etc.
                }
                break;

            // Agent is travelling from Bike Station A to Bike Station B
            case AGENT_STATUS.TRAVELLING:
                if (waitedForRecast)
                {
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        waitedForRecast = false;

                        BikeStation station = NextDestination.GetComponent<BikeStation>();
                        if (station.bikeStock < station.maxBikes)
                            ++station.bikeStock;

                        SetDestination(finalDestination.gameObject, finalDestination.ArriveRadius);
                        AgentStatus = AGENT_STATUS.ARRIVING;
                    }
                }
                else
                {
                    waitedForRecast = true;
                }
                break;

            // Agent walks from Bike Station B to Destination
            case AGENT_STATUS.ARRIVING:
                if (waitedForRecast)
                {
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        Destroy(gameObject);
                        //TODO: Reduce pollution etc
                    }
                }
                else
                {
                    waitedForRecast = true;
                }
                break;

            case AGENT_STATUS.NONE:
                Debug.LogError("Agent has STATUS::NONE!");
                break;

            default:
                break;
        }
    }

    // Called Upon Agent Spawn or when Arrives at bike station
    public void SetDestination(GameObject destination, float arrive_radius)
    {
        startedWaitingAt = Time.time;

        if (m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();

        if (destination != null)
        {
            NextDestination = destination;

            Vector2 random_unit_circle = Random.insideUnitCircle;
            Vector3 dest = new Vector3(random_unit_circle.x, 0.0f, random_unit_circle.y) * arrive_radius;
            m_Agent.destination = NextDestination.transform.position + dest;

            waitedForRecast = false;
        }
        else
        {
            Debug.LogError("Destination was NULL!");
            AgentStatus = AGENT_STATUS.NONE;
        }
    }

    // - LUCHO LEGACY -
    //public void SetFinalDestination(InterestPoint destination)
    //{
    //    if(destination != null)
    //        finalDestination = destination;
    //}

    //public void SendToBikeStation(BikeStation bikeStation)
    //{
    //    // 3. Look for the nearest bike station with bikes
    //    int bikestation_index = -1;
    //    if (nearbyBikeStations.Count > 0)
    //    {
    //        int i = 1;
    //        float distance = (nearbyBikeStations[0].transform.position - transform.position).magnitude;
    //        foreach (BikeStation bike_st in nearbyBikeStations)
    //        {
    //            if (bike_st.bikeStock > 0)
    //            {
    //                float new_distance = (bike_st.transform.position - transform.position).magnitude;
    //                if (new_distance < distance)
    //                {
    //                    bikestation_index = i;
    //                    distance = new_distance;
    //                }
    //            }

    //            ++i;
    //        }
    //    }

    //    // Pass the bike station to the cyclist (or keep it null if -1)
    //    if (nearbyBikeStations.Count > 0)
    //    {
            
    //    }

    //    //if (bikestation_index != -1)
    //    //    new_cyclist.ChangeDestination(m_BikeStationsInRange[bikestation_index], AIAgent.AGENT_STATUS.WALKING);
    //    //else
    //    //    new_cyclist.ChangeDestination(null, AIAgent.AGENT_STATUS.APT_WAIT);
    //}
}